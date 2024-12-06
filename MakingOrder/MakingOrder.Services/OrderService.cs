using MakingOrder.Models;
using MakingOrder.Repositories;
using MakingOrder.WebModel;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using RabbitMQ.Client;

namespace MakingOrder.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {

            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        public void Create(CreateOrderRequestAuth request)
        {
            var totalAmount = 0;

            var orderProducts = request.Products.Select(p =>
            {
                var product = _productRepository.GetById(p.ProductId);
                var checkStock = 0;

                if (product == null)
                {
                    throw new Exception($"Product with ID {p.ProductId} not found.");
                }

                totalAmount += p.Quantity * product.Price;
                checkStock = product.StockQuantity - p.Quantity;
                if (checkStock < 0)
                {
                    throw new Exception($"Stock quantity not available");
                }
                else
                {

                    product.StockQuantity -= p.Quantity;
                }

                return new OrderProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                };
            }).ToList();

            var order = new Order
            {
                OrderId = request.OrderId,
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                TotalAmount = totalAmount,
                OrderProducts = orderProducts
            };

            // using rabbitmq
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("email-box", ExchangeType.Direct);
            channel.QueueDeclare("email-send", false, false, false);
            channel.QueueBind("email-send", "email-box", "email-add");
            // htmlContent
            var emailBody = MakingEmailBody(request.Products);
            //var convertedJson = System.Text.Json.JsonSerializer.Serialize(emailBody);
            //var convertedByte = System.Text.Encoding.UTF8.GetBytes(convertedJson);
            //pass to queue
            var email = new EmailInfo { Email = request.Email, Body = emailBody };
            var emailJson = System.Text.Json.JsonSerializer.Serialize(email);
            channel.BasicPublish("email-box", "email-add",null, System.Text.Encoding.UTF8.GetBytes(emailJson));
            //create order, not send email
            _orderRepository.Create(order);
        }
        public List<Order> GetAll()
        {
            _orderRepository.GetAll();
            return _orderRepository.GetAll();
        }

        public List<Order> GetYourOrders(int customerID)
        {
            var orders = _orderRepository.GetAll();
            return orders.Where(o => o.CustomerId == customerID).ToList();
        }

        public string MakingEmailBody(List<OrderProductRequest> orderedProducts)
        {

            string htmlContent = @"
    <html>
        <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <h2 style='color: #4CAF50;'>Thank you for your order!</h2>
            <p style='font-size: 14px;'>We are delighted to serve you. Your order has been successfully placed, and here are the details:</p>
            <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                <thead>
                    <tr>
                        <th style='text-align: left; padding: 8px; border: 1px solid #ddd; background-color: #f2f2f2;'>Product Name</th>
                        <th style='text-align: left; padding: 8px; border: 1px solid #ddd; background-color: #f2f2f2;'>Quantity</th>
                    </tr>
                </thead>
                <tbody>";

            // Dynamically add rows for each product
            foreach (var orderedproduct in orderedProducts)
            {
                var product = _productRepository.GetByID(orderedproduct.ProductId);
                htmlContent += $@"
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{product.ProductName}</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>{orderedproduct.Quantity}</td>
                    </tr>";
            }

            htmlContent += @"
                </tbody>
            </table>
            <p style='margin-top: 20px; font-size: 14px;'>We hope you enjoy your purchase! If you have any questions, feel free to contact us.</p>
            <p style='font-size: 14px;'>Best regards,<br>Chroma Shop Service</p>
        </body>
    </html>";

            return htmlContent;
        }


        public void SendEmail(string htmlContent, string sentEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("chromashopservice@gmail.com"));
            email.To.Add(MailboxAddress.Parse(sentEmail));
            email.Subject = "Order Confirmation";
            // Attach the HTML content to the email body
            email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

            string apppassword = "mkgv mmvc imfw kxvl";
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("chromashopservice@gmail.com", apppassword);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
