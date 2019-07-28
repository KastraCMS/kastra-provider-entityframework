namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraMailTemplate
    {
        public int MailTemplateId { get; set; }
        public string Keyname { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}