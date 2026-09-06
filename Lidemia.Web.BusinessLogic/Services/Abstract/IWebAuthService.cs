// Created on 29/04/2020 18:14 by Andrey Laserson

using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.Web.BusinessLogic.Services.Abstract;

public interface IWebAuthService
{
    Task AuthCustomer(SignInResult<CustomerModel> result, CancellationToken cancellationToken);

    Task AuthSupplier(SignInResult<SupplierModel> result, CancellationToken cancellationToken);

    Task AuthAdmin(UserModel user, CancellationToken cancellationToken);

    Task SignOut(CancellationToken cancellationToken);
}