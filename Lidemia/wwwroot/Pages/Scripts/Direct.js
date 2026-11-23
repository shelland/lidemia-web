var Direct = Direct || (function () {

    var pickers = {
        products: {
            url: "products",
            title: "Supplier products",
            searchPlaceholder: "Search products...",
            empty: "The supplier does not have any products yet.",
            noResults: "No products match your search.",
            type: "Product",
            idField: "productId",
            getText: function (product) {
                return product.title || "";
            }
        },
        orders: {
            url: "orders",
            title: "Customer orders",
            searchPlaceholder: "Search orders...",
            empty: "The customer does not have any active orders with this supplier yet.",
            noResults: "No orders match your search.",
            type: "Order",
            idField: "orderId",
            getText: function (order) {
                return order.number || "";
            }
        }
    };

    return {

        init: function (data) {
            Direct.urls = data.urls;
            Direct.values = data.values;
            Direct.values.chatId = String(Direct.values.chatId);
            Direct.hub = null;
            Direct.pickerMode = null;
            Direct.pickerItems = [];

            Direct.bindEvents();

            if (Direct.values.chatId !== "0") {
                Direct.scrollToBottom();
            }

            Direct.connect();
        },

        bindEvents: function () {
            const form = document.querySelector("#directMessageForm");

            if (form) {
                form.addEventListener("submit", function (evt) {
                    evt.preventDefault();
                    Direct.send();
                });
            }

            const pickerSearch = document.querySelector("#directPickerSearch");

            if (pickerSearch) {
                pickerSearch.addEventListener("input", function () {
                    Direct.filterPickerItems(this.value);
                });
            }

            document.addEventListener("visibilitychange", function () {
                if (!document.hidden) {
                    Direct.markRead();
                }
            });
        },

        connect: async function () {
            if (typeof signalR === "undefined") {
                console.error("The SignalR client library is not loaded.");
                return;
            }

            Direct.hub = new signalR.HubConnectionBuilder()
                .withUrl(Direct.urls.hub)
                .withAutomaticReconnect()
                .build();

            Direct.hub.on("ReceiveMessage", Direct.onReceiveMessage);
            Direct.hub.on("ChatUpdated", Direct.onChatUpdated);
            Direct.hub.on("ChatCreated", Direct.onChatUpdated);

            Direct.hub.onreconnected(function () {
                Direct.joinChat();
            });

            try {
                await Direct.hub.start();

                if (Direct.values.chatId !== "0") {
                    await Direct.joinChat();
                }
            }
            catch (error) {
                console.error("Unable to connect to the direct chat hub.", error);
            }
        },

        joinChat: function () {
            if (!Direct.hub || Direct.values.chatId === "0") {
                return;
            }

            return Direct.hub.invoke("JoinChat", parseInt(Direct.values.chatId, 10))
                .catch(function (error) {
                    console.error("Unable to join the chat group.", error);
                });
        },

        send: async function () {
            const input = document.querySelector("#directMessageInput");

            if (!input || Direct.values.chatId === "0") {
                return;
            }

            const text = input.value.trim();

            if (!text) {
                return;
            }

            const command = text.toLowerCase();

            // A command is not a message, it opens the corresponding picker instead.
            if (command === "/products" || command === "/orders") {
                input.value = "";
                Direct.openPicker(command.substring(1));
                return;
            }

            input.value = "";
            input.focus();

            const response = await window.appFetch(Direct.urls.send, {
                method: "POST",
                body: JSON.stringify({
                    chatId: Direct.values.chatId,
                    type: "Message",
                    text: text
                })
            });

            if (!response.ok) {
                input.value = text;
                return;
            }

            const result = await response.json();

            if (!result.isSuccess || !result.data) {
                input.value = text;
                return;
            }

            Direct.onReceiveMessage(result.data);
        },

        openPicker: async function (mode) {
            const picker = pickers[mode];

            if (!picker || Direct.values.chatId === "0") {
                return;
            }

            const modalElement = document.querySelector("#directPickerModal");

            if (!modalElement) {
                return;
            }

            Direct.pickerMode = mode;
            Direct.pickerItems = [];

            const title = document.querySelector("#directPickerTitle");
            const search = document.querySelector("#directPickerSearch");

            if (title) {
                title.textContent = picker.title;
            }

            if (search) {
                search.value = "";
                search.placeholder = picker.searchPlaceholder;
            }

            Direct.renderPickerMessage("Loading...");

            bootstrap.Modal.getOrCreateInstance(modalElement).show();

            const response = await window.appFetch(`${Direct.urls[picker.url]}?chatId=${Direct.values.chatId}`, {
                method: "GET"
            });

            if (!response.ok) {
                Direct.renderPickerMessage("Unable to load the list.");
                return;
            }

            const result = await response.json();

            if (!result.isSuccess) {
                Direct.renderPickerMessage("Unable to load the list.");
                return;
            }

            Direct.pickerItems = result.data || [];
            Direct.renderPickerItems(Direct.pickerItems);
        },

        sendPickerItem: async function (id) {
            const picker = pickers[Direct.pickerMode];

            if (!picker || !id || Direct.values.chatId === "0") {
                return;
            }

            const payload = {
                chatId: Direct.values.chatId,
                type: picker.type
            };

            payload[picker.idField] = String(id);

            const response = await window.appFetch(Direct.urls.send, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                return;
            }

            const result = await response.json();

            if (!result.isSuccess || !result.data) {
                return;
            }

            Direct.closePicker();
            Direct.onReceiveMessage(result.data);
        },

        closePicker: function () {
            const modalElement = document.querySelector("#directPickerModal");

            if (!modalElement) {
                return;
            }

            bootstrap.Modal.getOrCreateInstance(modalElement).hide();
        },

        renderPickerItems: function (items) {
            const list = document.querySelector("#directPickerList");
            const picker = pickers[Direct.pickerMode];

            if (!list || !picker) {
                return;
            }

            list.innerHTML = "";

            if (!items || items.length === 0) {
                Direct.renderPickerMessage(picker.empty);
                return;
            }

            items.forEach(function (item) {
                list.append(Direct.createPickerItem(item, picker));
            });
        },

        renderPickerMessage: function (message) {
            const list = document.querySelector("#directPickerList");

            if (!list) {
                return;
            }

            list.innerHTML = "";

            const text = document.createElement("p");
            text.className = "text-muted small mb-0";
            text.textContent = message;

            list.append(text);
        },

        filterPickerItems: function (value) {
            const picker = pickers[Direct.pickerMode];
            const term = (value || "").trim().toLowerCase();

            if (!picker) {
                return;
            }

            if (!term) {
                Direct.renderPickerItems(Direct.pickerItems);
                return;
            }

            const filtered = Direct.pickerItems.filter(function (item) {
                return picker.getText(item).toLowerCase().includes(term);
            });

            if (filtered.length === 0) {
                Direct.renderPickerMessage(picker.noResults);
                return;
            }

            Direct.renderPickerItems(filtered);
        },

        createPickerItem: function (item, picker) {
            const button = document.createElement("button");
            button.type = "button";
            button.className = "direct-picker-item";

            button.addEventListener("click", function () {
                Direct.sendPickerItem(item.id);
            });

            if (picker === pickers.products && item.photoUrl) {
                const image = document.createElement("img");
                image.className = "direct-picker-item-image";
                image.src = item.photoUrl;
                image.alt = item.title || "";
                button.append(image);
            }

            if (picker === pickers.orders) {
                const icon = document.createElement("span");
                icon.className = "direct-picker-item-icon";
                icon.innerHTML = '<i class="uil uil-box"></i>';
                button.append(icon);
            }

            const body = document.createElement("span");
            body.className = "direct-picker-item-body";

            const title = document.createElement("span");
            title.className = "direct-picker-item-title";
            title.textContent = item.title || item.number || "Item";
            body.append(title);

            const subtitle = document.createElement("span");
            subtitle.className = "direct-picker-item-subtitle";

            if (picker === pickers.products) {
                subtitle.textContent = item.price || "";
            }
            else {
                subtitle.textContent = [item.status, item.totalSum, Direct.formatItemCount(item.itemCount)]
                    .filter(function (part) {
                        return !!part;
                    })
                    .join(" · ");
            }

            body.append(subtitle);
            button.append(body);

            return button;
        },

        onReceiveMessage: function (message) {
            if (!message) {
                return;
            }

            if (String(message.chatId) !== Direct.values.chatId) {
                Direct.updatePreview({
                    chatId: message.chatId,
                    lastMessageText: Direct.getMessagePreviewText(message),
                    lastMessageDate: message.createDate
                });

                return;
            }

            Direct.appendMessage(message);
            Direct.updatePreview({
                chatId: message.chatId,
                lastMessageText: Direct.getMessagePreviewText(message),
                lastMessageDate: message.createDate
            });
            Direct.scrollToBottom();

            if (message.senderType !== Direct.values.currentRole) {
                Direct.markRead();
            }
        },

        getMessagePreviewText: function (message) {
            if (message.type === "Product") {
                return message.product && message.product.title ? message.product.title : "Shared product";
            }

            if (message.type === "Order") {
                return message.order && message.order.number ? message.order.number : "Shared order";
            }

            return message.text || "";
        },

        onChatUpdated: function (summary) {
            if (!summary) {
                return;
            }

            const chatId = String(summary.chatId);
            const isActiveChat = chatId === Direct.values.chatId;
            const hasMessage = !!summary.lastMessageText;

            let item = Direct.findChatListItem(chatId);

            if (!item) {
                item = Direct.prependChatListItem(summary);
            }
            else {
                Direct.updatePreview(summary);
            }

            // The summary is only pushed to the counterparty, so a present message always means
            // an unread message for this participant unless the conversation is currently open.
            if (hasMessage && !isActiveChat) {
                Direct.incrementUnread(chatId);
            }
        },

        findChatListItem: function (chatId) {
            return document.querySelector(`.direct-chat-list-item[data-chat-id="${chatId}"]`);
        },

        updatePreview: function (summary) {
            const item = Direct.findChatListItem(summary.chatId);

            if (!item) {
                return;
            }

            const preview = item.querySelector(".direct-chat-preview");

            if (preview) {
                preview.textContent = Direct.getPreviewText(summary);
            }

            const date = item.querySelector(".direct-chat-date");

            if (date) {
                date.textContent = Direct.formatListDate(summary.lastMessageDate);
            }
        },

        prependChatListItem: function (summary) {
            const list = document.querySelector(".direct-chat-list");

            if (!list) {
                return null;
            }

            const empty = list.querySelector(".direct-chat-list-empty");

            if (empty) {
                empty.remove();
            }

            const name = Direct.getCounterpartyName(summary);
            const isActiveChat = String(summary.chatId) === Direct.values.chatId;

            const item = document.createElement("a");
            item.className = "direct-chat-list-item" + (isActiveChat ? " active" : "");
            item.href = `${Direct.urls.index}?chatId=${summary.chatId}`;
            item.dataset.chatId = summary.chatId;

            const avatar = document.createElement("span");
            avatar.className = "avatar bg-pink text-white w-9 h-9 fs-15";
            avatar.textContent = Direct.getInitials(name);

            const body = document.createElement("span");
            body.className = "flex-grow-1 min-w-0";

            const header = document.createElement("span");
            header.className = "d-flex justify-content-between align-items-center gap-2";

            const nameElement = document.createElement("span");
            nameElement.className = "fw-bold text-truncate";
            nameElement.textContent = name;

            const date = document.createElement("span");
            date.className = "small text-muted text-nowrap direct-chat-date";
            date.textContent = Direct.formatListDate(summary.lastMessageDate);

            const footer = document.createElement("span");
            footer.className = "d-flex justify-content-between align-items-center gap-2";

            const preview = document.createElement("span");
            preview.className = "small text-muted text-truncate direct-chat-preview";
            preview.textContent = Direct.getPreviewText(summary);

            const badge = document.createElement("span");
            badge.className = "badge bg-primary rounded-pill direct-chat-unread";

            const hasUnread = !!summary.lastMessageText && !isActiveChat;
            badge.textContent = hasUnread ? "1" : "0";
            badge.classList.toggle("d-none", !hasUnread);

            header.append(nameElement, date);
            footer.append(preview, badge);
            body.append(header, footer);
            item.append(avatar, body);
            list.prepend(item);

            return item;
        },

        getCounterpartyName: function (summary) {
            return Direct.values.currentRole === "Supplier" ? summary.customerName : summary.supplierName;
        },

        getPreviewText: function (summary) {
            return summary.lastMessageText ? summary.lastMessageText : "No messages yet";
        },

        getInitials: function (name) {
            if (!name) {
                return "?";
            }

            const parts = name.trim().split(/\s+/).slice(0, 2);

            return parts.map(function (part) {
                return part.charAt(0).toUpperCase();
            }).join("");
        },

        incrementUnread: function (chatId) {
            const item = Direct.findChatListItem(chatId);

            if (!item) {
                return;
            }

            const badge = item.querySelector(".direct-chat-unread");

            if (!badge) {
                return;
            }

            const count = parseInt(badge.textContent || "0", 10) + 1;
            badge.textContent = count;
            badge.classList.remove("d-none");
        },

        appendMessage: function (message) {
            const container = document.querySelector("#directMessages");

            if (!container || !message.id) {
                return;
            }

            if (container.querySelector(`[data-message-id="${message.id}"]`)) {
                return;
            }

            const empty = container.querySelector(".direct-chat-empty");

            if (empty) {
                empty.classList.add("d-none");
            }

            const isMine = message.senderType === Direct.values.currentRole;

            const wrapper = document.createElement("div");
            wrapper.className = "direct-message " + (isMine ? "outgoing" : "incoming");
            wrapper.dataset.messageId = message.id;

            const bubble = document.createElement("div");
            bubble.className = "direct-message-bubble";

            if (message.type === "Product") {
                if (message.product) {
                    bubble.append(Direct.createProductCard(message.product));
                }
                else {
                    bubble.append(Direct.createUnavailable("This product is not available anymore."));
                }
            }
            else if (message.type === "Order") {
                if (message.order) {
                    bubble.append(Direct.createOrderCard(message.order));
                }
                else {
                    bubble.append(Direct.createUnavailable("This order is not available anymore."));
                }
            }
            else {
                const text = document.createElement("p");
                text.className = "mb-0";
                text.textContent = message.text || "";
                bubble.append(text);
            }

            const time = document.createElement("span");
            time.className = "direct-message-time";
            time.textContent = Direct.formatDate(message.createDate);

            bubble.append(time);
            wrapper.append(bubble);
            container.append(wrapper);
        },

        createUnavailable: function (text) {
            const paragraph = document.createElement("p");
            paragraph.className = "mb-0 fst-italic";
            paragraph.textContent = text;

            return paragraph;
        },

        createProductCard: function (product) {
            const card = document.createElement("a");
            card.className = "direct-product-card";
            card.href = product.url || "#";
            card.target = "_blank";
            card.rel = "noopener";

            if (product.photoUrl) {
                const image = document.createElement("img");
                image.className = "direct-product-card-image";
                image.src = product.photoUrl;
                image.alt = product.title || "";
                card.append(image);
            }

            const body = document.createElement("span");
            body.className = "direct-product-card-body";

            const title = document.createElement("span");
            title.className = "direct-product-card-title";
            title.textContent = product.title || "Product";
            body.append(title);

            if (product.price) {
                const price = document.createElement("span");
                price.className = "direct-product-card-price";
                price.textContent = product.price;
                body.append(price);
            }

            card.append(body);
            return card;
        },

        createOrderCard: function (order) {
            const card = document.createElement("a");
            card.className = "direct-order-card";
            card.href = order.url || "#";

            const icon = document.createElement("span");
            icon.className = "direct-order-card-icon";
            icon.innerHTML = '<i class="uil uil-box"></i>';
            card.append(icon);

            const body = document.createElement("span");
            body.className = "direct-order-card-body";

            const header = document.createElement("span");
            header.className = "direct-order-card-header";

            const number = document.createElement("span");
            number.className = "direct-order-card-number";
            number.textContent = order.number || "Order";
            header.append(number);

            if (order.status) {
                const status = document.createElement("span");
                status.className = "badge bg-soft-primary text-primary";
                status.textContent = order.status;
                header.append(status);
            }

            const meta = document.createElement("span");
            meta.className = "direct-order-card-meta";

            [order.totalSum, Direct.formatItemCount(order.itemCount), Direct.formatShortDate(order.createDate)]
                .filter(function (part) {
                    return !!part;
                })
                .forEach(function (part) {
                    const element = document.createElement("span");
                    element.textContent = part;
                    meta.append(element);
                });

            body.append(header, meta);
            card.append(body);

            return card;
        },

        markRead: async function () {
            if (!Direct.values.chatId || Direct.values.chatId === "0") {
                return;
            }

            await window.appFetch(`${Direct.urls.read}?chatId=${Direct.values.chatId}`, {
                method: "POST"
            });

            Direct.clearUnread(Direct.values.chatId);
        },

        clearUnread: function (chatId) {
            const badge = document.querySelector(`.direct-chat-list-item[data-chat-id="${chatId}"] .direct-chat-unread`);

            if (!badge) {
                return;
            }

            badge.textContent = "0";
            badge.classList.add("d-none");
        },

        scrollToBottom: function () {
            const container = document.querySelector("#directMessages");

            if (container) {
                container.scrollTop = container.scrollHeight;
            }
        },

        formatDate: function (value) {
            if (!value) {
                return "";
            }

            const date = new Date(value);

            if (isNaN(date.getTime())) {
                return "";
            }

            return date.toLocaleString(undefined, {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit"
            });
        },

        formatItemCount: function (count) {
            const value = parseInt(count || "0", 10);

            if (isNaN(value) || value <= 0) {
                return "";
            }

            return value === 1 ? "1 item" : `${value} items`;
        },

        formatShortDate: function (value) {
            if (!value) {
                return "";
            }

            const date = new Date(value);

            if (isNaN(date.getTime())) {
                return "";
            }

            return date.toLocaleDateString(undefined, {
                day: "2-digit",
                month: "2-digit",
                year: "numeric"
            });
        },

        formatListDate: function (value) {
            if (!value) {
                return "";
            }

            const date = new Date(value);

            if (isNaN(date.getTime())) {
                return "";
            }

            return date.toLocaleString(undefined, {
                day: "2-digit",
                month: "2-digit",
                hour: "2-digit",
                minute: "2-digit"
            });
        }

    };

})();
