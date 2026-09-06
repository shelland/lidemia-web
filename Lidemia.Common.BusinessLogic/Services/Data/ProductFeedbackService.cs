// Created on 03/09/2026 18:55 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IProductFeedbackService>(ServiceLifetime.Scoped)]
public class ProductFeedbackService : IProductFeedbackService
{
    private readonly IProductFeedbackRepository productFeedbackRepository;

    public ProductFeedbackService(IProductFeedbackRepository productFeedbackRepository)
    {
        this.productFeedbackRepository = productFeedbackRepository;
    }
}