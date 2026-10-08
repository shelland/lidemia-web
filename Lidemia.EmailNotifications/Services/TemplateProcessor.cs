using Lidemia.Resources.App;
using Scriban;
using System.Text;

namespace Lidemia.EmailNotifications.Services;

internal class TemplateProcessor
{
    private static string GetBaseTemplate()
    {
        return Encoding.UTF8.GetString(AppResources.BaseTemplate);
    }

    public static string GetSupplierSignUpEmailTemplate(string name)
    {
        var baseTemplateHtml = GetBaseTemplate();
        var emailTemplateHtml = Encoding.UTF8.GetString(AppResources.SupplierSignUpEmailTemplate);

        var contentTemplate = Template.Parse(emailTemplateHtml);
        var renderedTemplateContent = contentTemplate.Render(new
        {
            Name = name
        });

        var emailTemplate = Template.Parse(baseTemplateHtml);
        var emailContent = emailTemplate.Render(new
        {
            Content = renderedTemplateContent
        });

        return emailContent;
    }
}