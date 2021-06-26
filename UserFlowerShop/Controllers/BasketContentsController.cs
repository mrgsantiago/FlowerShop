using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using UserFlowerShop.Models;

namespace UserFlowerShop.Controllers
{
    public class BasketContentsController : Controller
    {
        private FlowerEntities db = new FlowerEntities();

        // GET: BasketContents
        public ActionResult Index()
        {

            int idUser = db.Users.Where(p => p.Mail == User.Identity.Name).FirstOrDefault().Id;
            int IdBasket = db.Basket.Where(p => p.IdUser == idUser).FirstOrDefault().ID;
            var basketContent = db.BasketContent.Where(p => p.IDBasket == IdBasket);
            if(basketContent != null)
            {
                ViewBag.IssuePoints = new SelectList(db.Points, "ID", "Address");
                ViewBag.PayMethod = new SelectList(db.PaymentType, "ID", "PaymentType1");
                return View(basketContent.ToList());
            }
            else
            {
                return Json(new { list = true });
            }
           
        }

        public void CreateOrder(string pay, string point)
        {
            int idUser = db.Users.Where(p => p.Mail == User.Identity.Name).FirstOrDefault().Id;
            int idBasket = db.Basket.Where(p => p.IdUser == idUser).FirstOrDefault().ID;
            PDFCreate(idBasket);
            Order order = new Order { IdBasket = idBasket, IdPayment = Convert.ToInt32(pay), IdStatus = 1, IdPoints = Convert.ToInt32(point) };
            db.Order.Add(order);
            db.SaveChanges();
            db.BasketContent.RemoveRange(db.BasketContent.Where(x => x.IDBasket == idBasket));
            db.SaveChanges();
            string path = Server.MapPath(Url.Content("~/Content/PDF/Consist.pdf"));
            SendEmailAsync(db.Users.Where(p => p.Mail == User.Identity.Name).FirstOrDefault().Mail, path);
        }
        private void SendEmailAsync(string Email, string path) // Метод для отправки сообщения и PDF-файла с содержимым заказа на почту клиенту
        {
            try
            {
                MailAddress from = new MailAddress("rita2112gorb@gmail.com", "Интернет-магазин цветов 'SILKEN'");
                MailAddress to = new MailAddress(Email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Ваш заказ оформлен";
                m.Body = $"Здравствуйте. Ваш заказ оформлен. Вы можете просмотреть купленные товары в прикреплённом документе. С уважением, интернет-магазин цветов 'SILKEN'";
                m.Attachments.Add(new Attachment(path));
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("rita2112gorb@gmail.com", "Margo15082002");
                smtp.EnableSsl = true;
                smtp.Send(m);

            }
            catch (Exception ex)
            {

            }
        }
        private void PDFCreate(int idBasket) // Метод для формирования PDF-фвйла с содержимым заказа
        {
            try
            {
                var basket_Consist = db.BasketContent.Include(b => b.Basket).Include(b => b.Goods).Where(p => p.IDBasket == idBasket);


                DataTable tableConsist = new DataTable();
                tableConsist.Columns.Add("Name");
                tableConsist.Columns.Add("Amount");
                tableConsist.Columns.Add("Price");
                decimal Price = 0;
                foreach (var item in basket_Consist)
                {
                    Price += item.Price;
                    DataRow row = tableConsist.NewRow();
                    row["Name"] = item.Goods.Name;
                    row["Amount"] = item.Quantity;
                    row["Price"] = item.Price;
                    tableConsist.Rows.Add(row);

                }
                string dest = Server.MapPath(Url.Content("~/Content/PDF/Consist.pdf"));
                PdfWriter writer = new PdfWriter(dest);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document document = new Document(pdfDoc);
                Table table = new Table(tableConsist.Columns.Count);
                PdfFont russian = PdfFontFactory.CreateFont(Server.MapPath(Url.Content("~/Content/Font/Arial.ttf")), "CP1251", true);
                document.SetFont(russian);
                table.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                table.SetFontColor(new DeviceRgb(0, 0, 0));
                iText.Kernel.Colors.Color color = new DeviceRgb(255, 228, 196);
                CreateCell(table, "Наименование товара", 250, color);
                CreateCell(table, "Количество", 100, color);
                CreateCell(table, "Цена, руб.", 100, color);

                for (int i = 0; i < tableConsist.Rows.Count; i++)
                {
                    for (int j = 0; j < tableConsist.Columns.Count; j++)
                    {
                        Cell cell = new Cell();
                        if (j == 2) cell.Add(new Paragraph(Convert.ToDecimal(tableConsist.Rows[i][j]).ToString().Remove(tableConsist.Rows[i][j].ToString().Length - 5, 5)));

                        else cell.Add(new Paragraph(tableConsist.Rows[i][j].ToString()));
                        cell.SetBackgroundColor(new DeviceRgb(255, 248, 220));
                        cell.SetFontSize(12);
                        table.AddCell(cell);
                    }
                }
                iText.Layout.Element.Paragraph price = new Paragraph("Итого: " + " " + Price.ToString().Remove(Price.ToString().Length - 5, 5) + " руб.");
                document.Add(table);
                document.Add(price);
                document.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        private void CreateCell(Table table, string str, float widthCell, iText.Kernel.Colors.Color bgColor)
        {
            Cell cell1 = new Cell();
            cell1.Add(new Paragraph(str));
            cell1.SetWidth(widthCell);
            cell1.SetFontSize(12);
            cell1.SetBackgroundColor(bgColor);
            table.AddCell(cell1);
        }


        // GET: BasketContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasketContent basketContent = db.BasketContent.Find(id);
            if (basketContent == null)
            {
                return HttpNotFound();
            }
            return View(basketContent);
        }

        // GET: BasketContents/Create
        public ActionResult Create()
        {
            ViewBag.IDBasket = new SelectList(db.Basket, "ID", "ID");
            ViewBag.IDGoods = new SelectList(db.Goods, "ID", "Name");
            return View();
        }

        // POST: BasketContents/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string IDGoods, string Amount, string Price)
        {
            int idGoods = Convert.ToInt32(IDGoods);
            int AcceptableAmount = 0;
            bool Presence = db.Goods.Where(p => p.ID == idGoods).FirstOrDefault().Availability;
            if (Presence)
            {
                int amount = Convert.ToInt32(Amount);
                decimal price = Convert.ToDecimal(Price);
                decimal endPrice = amount * price;
                int id = db.Users.Where(p => p.Mail == User.Identity.Name).FirstOrDefault().Id;
                int idBasket = db.Basket.Where(p => p.IdUser == id).FirstOrDefault().ID;
                var item = db.BasketContent.Where(p => p.IDBasket == idBasket && p.IDGoods == idGoods).FirstOrDefault();

                if (item != null)
                {
                    int MaxAmount = db.Goods.Where(p => p.ID == item.IDGoods).FirstOrDefault().Quantity;
                    int LimitAmount = amount + item.Quantity;
                    if (LimitAmount > MaxAmount)
                    {
                        AcceptableAmount = LimitAmount - MaxAmount;
                        AcceptableAmount = amount - AcceptableAmount;
                        return Json(new { Presence, AcceptableAmount });

                    }
                    item.Quantity += amount;
                    item.Price += endPrice;
                    db.SaveChanges();
                    //var medicine = db.Medicines.Where(p => p.ID_Medicine == IDMedicines).FirstOrDefault();
                    //medicine.Amount -= amount;
                    //if (medicine.Amount == 0)
                    //{
                    //    medicine.Presence = false;
                    //}
                    //db.SaveChanges();

                }
                else
                {
                    BasketContent basket = new BasketContent { IDBasket = idBasket, IDGoods = idGoods, Quantity = amount, Price = endPrice };
                    db.BasketContent.Add(basket);
                    db.SaveChanges();
                    //var medicine = db.Medicines.Where(p => p.ID_Medicine == IDMedicines).FirstOrDefault();
                    //medicine.Amount -= amount;
                    //if (medicine.Amount == 0)
                    //{
                    //    medicine.Presence = false;
                    //}
                    //db.SaveChanges();
                }
            }
            return Json(new { Presence, AcceptableAmount });
        }
        public ActionResult ChangeElement(string idGood, string amount)
        {
            int IDGood = Convert.ToInt32(idGood);
            int Amount = Convert.ToInt32(amount);
            decimal priceOne = db.Goods.Where(p => p.ID == IDGood).FirstOrDefault().Price;
            decimal newPrice = priceOne * Amount;
            int idUser = db.Users.Where(p => p.Mail == User.Identity.Name).FirstOrDefault().Id;
            int idBasket = db.Basket.Where(p => p.IdUser == idUser).FirstOrDefault().ID;
            var row = db.BasketContent.Where(p => p.IDBasket == idBasket && p.IDGoods == IDGood).FirstOrDefault();
            row.Price = newPrice;
            row.Quantity = Amount;
            db.SaveChanges();
            return Json(new { newPrice });
        }

        // GET: BasketContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasketContent basketContent = db.BasketContent.Find(id);
            if (basketContent == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDBasket = new SelectList(db.Basket, "ID", "ID", basketContent.IDBasket);
            ViewBag.IDGoods = new SelectList(db.Goods, "ID", "Name", basketContent.IDGoods);
            return View(basketContent);
        }

        // POST: BasketContents/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDBasket,IDGoods,Quantity,Price")] BasketContent basketContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basketContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDBasket = new SelectList(db.Basket, "ID", "ID", basketContent.IDBasket);
            ViewBag.IDGoods = new SelectList(db.Goods, "ID", "Name", basketContent.IDGoods);
            return View(basketContent);
        }

        // GET: BasketContents/Delete/5
       

        // POST: BasketContents/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            int Id = Convert.ToInt32(id);
            BasketContent basketContent = db.BasketContent.Where(p => p.ID == Id).FirstOrDefault();
            int IdGood = basketContent.IDGoods;
            int Amount = basketContent.Quantity;
            db.BasketContent.Remove(basketContent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
