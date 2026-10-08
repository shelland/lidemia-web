var CreateProduct = CreateProduct || (function () {

	return {

		init: function (data) {
			CreateProduct.urls = data.urls;
			CreateProduct.value = data.values;

			this.bindEvents();
			this.initPlugins();
		},

		bindEvents: function () {

		},

		initPlugins: function () {
			var regExp = Shared.getHashTagRegExp();

			var tagify = new Tagify(document.getElementById("tags"), {
				pattern: regExp
            });

            this.initEditor();
		},

        initEditor: function () {

            const Header = window.Header;
            const List = window.EditorjsList; // В новых версиях плагин списков может называться NestedList
            const Checklist = window.Checklist;
            const Quote = window.Quote;
            const Table = window.Table;
            const Delimiter = window.Delimiter;
            const InlineCode = window.InlineCode;
            const Marker = window.Marker;
            const CodeTool = window.CodeTool; // Часто экспортируется как window.Code
            const RawTool = window.RawTool;
            const Embed = window.Embed;
            const Warning = window.Warning;
            const ImageTool = window.ImageTool;

            const editor = new EditorJS({
                holder: 'editorjs',
                placeholder: 'Нажмите Tab или плюсик, чтобы выбрать инструмент...',
                tools: {
                    header: {
                        class: Header,
                        config: { placeholder: 'Введите заголовок...', levels: [1, 2, 3, 4], defaultLevel: 2 }
                    },
                    list: {
                        class: List,
                        inlineToolbar: true
                    },
                    checklist: {
                        class: Checklist,
                        inlineToolbar: true
                    },
                    quote: {
                        class: Quote,
                        inlineToolbar: true,
                        config: { quotePlaceholder: 'Введите цитату', captionPlaceholder: 'Автор' }
                    },
                    table: {
                        class: Table,
                        inlineToolbar: true
                    },
                    delimiter: Delimiter,
                    inlineCode: {
                        class: InlineCode,
                    },
                    marker: {
                        class: Marker,
                    },
                    code: {
                        class: CodeTool,
                    },
                    raw: {
                        class: RawTool,
                    },
                    embed: {
                        class: Embed,
                        config: {
                            services: { youtube: true, coub: true, twitter: true, instagram: true }
                        }
                    },
                    warning: {
                        class: Warning,
                        inlineToolbar: true,
                        config: { titlePlaceholder: 'Название', messagePlaceholder: 'Текст предупреждения' }
                    },
                    image: {
                        class: ImageTool,
                        config: {
                            // Пример настройки эндпоинтов загрузки на ваш бэкенд:
                            // endpoints: { byFile: 'http://localhost:8008/uploadFile', byUrl: 'http://localhost:8008/fetchUrl' }
                        }
                    }
                },
                data: {
                    blocks: [
                        {
                            type: 'header',
                            data: { text: 'Все плагины Editor.js подключены', level: 1 }
                        }
                    ]
                }
            });

            editor.save().then((outputData) => {
                console.log('Article data: ', outputData)
            }).catch((error) => {
                console.log('Saving failed: ', error)
            });
		}

	}

})();