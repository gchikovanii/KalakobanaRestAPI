using Kalakobana.SharedKernel.Models;

namespace Kalakobana.Notifications.API.Infrastructure.HTMLs
{
    public class PasswordResetHTML
    {
        public static string GenerateBody(MessageContractEvent request)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Wise Buzzer</title>
        <style>
            body {{
                margin: 0;
                padding: 0;
                font-family: Arial, sans-serif;
                background: linear-gradient(to right, #0e7490, #1d4ed8); 
                color: white;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                text-align: center;
            }}
            .logo {{
                margin-bottom: 20px;
            }}
            .content {{
                background: rgba(0, 0, 0, 0.8);
                padding: 30px;
                border-radius: 10px;
            }}
            h1 {{
                color: #a5f3fc;
            }}
            p {{
                color: #93c5fd;
                line-height: 1.6;
            }}
            .button {{
                display: inline-block;
                background: linear-gradient(to right, #06b6d4, #3b82f6);
                padding: 15px 30px;
                color: white;
                text-decoration: none;
                border-radius: 5px;
                font-size: 16px;
                margin-top: 20px;
                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
                transition: background 0.3s ease;
            }}
            .button:hover {{
                background: linear-gradient(to right, #0891b2, #2563eb);
            }}
            .footer {{
                margin-top: 30px;
                font-size: 14px;
                color: #93c5fd;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='logo'>
                <img src='https://wise-buzzer.vercel.app/hero.png' alt='Wise Buzzer Logo' width='150'>
            </div>
            <div class='content'>
                <h1>პაროლის აღდგენა!</h1>
                <p>თქვენ მოითხოვეთ პროფილის აღდგენა, გთხოვთ მიყვეთ ინსტრუქციას, რათა წარმატებით აღადგინოთ წვდომა მომხარებელზე.</p>
                <p>დაჭირეთ ღილაკს ქვემოთ, რათა დაიწყოთ აღდგენის პროცესი:</p>
                <a href='{request.Link}' class='button'>აღდგენა</a>
                <p>იმ შემთხვევაში თუ ლინკი თქვენ არ მოგითხოვიათ, დაგვიკავშირდით ელ.ფოსტაზე: <a href='mailto:info.wisebuzzer@gmail.com' style='color:#60a5fa;'>info.wisebuzzer@gmail.com</a>.</p>
            </div>
            <div class='footer'>
                <p>&copy; 2024 Wise Buzzer. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";
        }
    }
}
