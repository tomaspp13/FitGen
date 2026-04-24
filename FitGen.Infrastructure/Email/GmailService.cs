using MailKit.Net.Smtp;
using MimeKit;
using FitGen.Dominio.Interfaces;

namespace FitGen.Infrastructure.Email
{
    public class GmailService : IEmailService
    {
        private readonly string _emailUsuario;
        private readonly string _emailContraseña;

        private string FormatearRutina(string contenido)
        {
            // Elimina los bloques de código markdown (``` al inicio y al final)
            contenido = System.Text.RegularExpressions.Regex.Replace(contenido, @"```.*?\n?", "").Trim();

            // Convierte **texto** en títulos con estilo
            contenido = System.Text.RegularExpressions.Regex.Replace(
                contenido,
                @"\*\*(.+?)\*\*",
                m => $"<h3 style='color: #6366f1; font-size: 15px; margin: 20px 0 8px; padding: 8px 12px; background: #eef2ff; border-radius: 6px;'>{m.Groups[1].Value}</h3>"
            );

            // Convierte saltos de línea en <br>
            contenido = contenido.Replace("\n", "<br>");

            return contenido;
        }
        public GmailService(string emailUsuario,string emailContraseña)
        {
            _emailUsuario = emailUsuario;
            _emailContraseña = emailContraseña;
        }

        public async Task EnviarRutinaAsync(string email, string contenidoRutina, string nombre, string apellido)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(_emailUsuario));
            mensaje.To.Add(MailboxAddress.Parse(email));
            mensaje.Subject = $"Tu rutina personalizada, {nombre}";

            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background: #f5f5f0;'>

                <div style='background: #6366f1; padding: 28px 24px; border-radius: 12px 12px 0 0;'>
                    <h1 style='color: white; margin: 0; font-size: 24px;'>FitGen</h1>
                    <p style='color: rgba(255,255,255,0.85); margin: 6px 0 0; font-size: 14px;'>Tu rutina personalizada está lista</p>
                </div>

                <div style='background: #ffffff; border: 1px solid #e5e5e0; border-top: none; padding: 28px 24px;'>
                    <p style='font-size: 16px; color: #1a1a1a; margin: 0 0 20px;'>
                        Hola, <strong>{nombre} {apellido}</strong> 👋
                    </p>
                    <p style='font-size: 14px; color: #555; margin: 0 0 24px; line-height: 1.6;'>
                        Preparamos una rutina especialmente para vos basada en tu perfil y objetivos. 
                        Seguila con constancia y vas a ver resultados.
                    </p>

                    <div style='background: #f8f8ff; border-left: 4px solid #6366f1; border-radius: 4px; padding: 20px 24px; margin-bottom: 24px;'>
                        {FormatearRutina(contenidoRutina)}
                    </div>

                    <p style='font-size: 13px; color: #888; line-height: 1.6; margin: 0;'>
                        Recordá consultar con un profesional antes de iniciar cualquier rutina de ejercicios.
                    </p>
                </div>

                <div style='background: #f5f5f0; border-radius: 0 0 12px 12px; padding: 16px 24px; text-align: center;'>
                    <p style='color: #aaa; font-size: 12px; margin: 0;'>Generado por <strong style='color: #6366f1;'>FitGen</strong></p>
                </div>

            </body>
            </html>";

            mensaje.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailUsuario, _emailContraseña);
            await smtp.SendAsync(mensaje);
            await smtp.DisconnectAsync(true);
        }
    }
}
