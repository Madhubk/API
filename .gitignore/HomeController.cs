using KYCExtraction.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KYCExtraction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          

            ErrorLog.SMTTrace("Img1" + "Test");
            KycFileModel obj = new KycFileModel();
            return View(obj);
        }
        [HttpPost]
        public ActionResult fileUpload(KycFileModel v)
        {
            //F:\Sharefolder-GIHOSP\v.KYCFileUpload
            //obj.Createdby = Convert.ToInt32(Session["UserID"]);
            var uploadDir = ConfigurationManager.AppSettings["KYCPath"];
            if (v.KYCFile != null && v.KYCFile.ContentLength > 0)
            {
                string strlocalPath = uploadDir;
                if (!System.IO.Directory.Exists(strlocalPath))
                {

                    System.IO.Directory.CreateDirectory(strlocalPath);
                }
                string extension = Path.GetExtension(v.KYCFile.FileName);
                string Uniquekey1 = DateTime.Now.ToString("MMddyyyyHHmmssfff");


                string filename = (Uniquekey1 + v.KYCFile.FileName);
                var imagePath = Path.Combine(uploadDir, filename);
                var imageUrl = Path.Combine(uploadDir, filename);
                v.KYCFile.SaveAs(imagePath);

                // ViewBag.src = " file://10.100.1.30///Sharefolder-GIHOSP/KYCFileUpload/" + filename;
                string postData = "imgfile=" + filename;
                // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction");
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.ASCII.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string pageContent = myStreamReader.ReadToEnd();

                v = JsonConvert.DeserializeObject<KycFileModel>(pageContent);

                //write text file
                //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;


            }
            //obj.DocumentProofPath = imageUrl;
            return View("Index", v);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Kycdocument()
        {

            KycFileModel obj = new KycFileModel();

            return View(obj);
        }

        public ActionResult BankStatementAnalysis()
        {
            ViewBag.result = "";
            BankStatementModel v = new BankStatementModel();
            //v.Name = "Thanu";
            //v.BankName="HDFC";
            //v.AccountStatementPeriod="JAN 2017- MAY 2018";
            //v.AccountNumber = "123456789";
            return View(v);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BankStatementdata(BankStatementModel v)
        {
            try
            {

                if (v.BankStateMentFile != null)
                {
                    var uploadDir = ConfigurationManager.AppSettings["BankStatementPath"];
                    if (v.BankStateMentFile != null && v.BankStateMentFile.ContentLength > 0)
                    {
                        string strlocalPath = uploadDir;
                        if (!System.IO.Directory.Exists(strlocalPath))
                        {

                            System.IO.Directory.CreateDirectory(strlocalPath);
                        }
                        string extension = Path.GetExtension(v.BankStateMentFile.FileName);
                        string Uniquekey2 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename = (Uniquekey2 + v.BankStateMentFile.FileName);
                        var imagePath = Path.Combine(uploadDir, filename);
                        var imageUrl = Path.Combine(uploadDir, filename);
                        v.BankStateMentFile.SaveAs(imagePath);


                        Session["imagdatatavalue"] = null;
                        Session["imagdatatavalue"] = "https://easetours.in/kycdata/KYCDocument/" + filename;

                        //ViewBag.src = "http://14.142.234.97/KYCDocument/" + filename;

                        ViewBag.src = "https://easetours.in/kycdata/KYCDocument/" + filename;
                      //  v.urldatavalue = "http://14.142.234.97/KYCDocument/" + filename;


                      //  ViewBag.src = "file://14.142.234.97///Sharefolder-GIHOSP/KYCFileUpload/" + filename;
                      //  ViewBag.src = " file://10.100.1.30///Sharefolder-GIHOSP/KYCFileUpload/" + filename;

                        //string postData = "imgfile=INBDQ03900002F_id_1.jpg";


                        string postData = "pdffile="+ filename;
                       // HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4005/kvbextraction");
                       // string postData = "pdffile=" + filename;
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4005/kvbextraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        v = JsonConvert.DeserializeObject<BankStatementModel>(pageContent);

                        string s = JsonConvert.SerializeObject(v);
                        ErrorLog.SMTTrace("Kycdocumentdata KYCFILERESONSE doc" + s);
                        ViewBag.result = "Sucess";
                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;
                        //ViewBag.src = "http://14.142.234.97/KYCDocument/" + filename;
                        // v.urldatavalue = "http://14.142.234.97/KYCDocument/" + filename;
                    }
                }

                return View("BankStatementAnalysis", v);
                } catch (Exception ex)
            {

                ErrorLog.SMTTrace("KYC Kycdocumentdataexception" + ex.Message);
            }
            return View("BankStatementAnalysis");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Kycdocumentdata(KycFileModel v)
        {

            try
            {

                if (v.KYCFile != null)
                {
                    var uploadDir = ConfigurationManager.AppSettings["KYCPath"];
                    if (v.KYCFile != null && v.KYCFile.ContentLength > 0)
                    {
                        string strlocalPath = uploadDir;
                        if (!System.IO.Directory.Exists(strlocalPath))
                        {

                            System.IO.Directory.CreateDirectory(strlocalPath);
                        }
                        string extension = Path.GetExtension(v.KYCFile.FileName);
                        string Uniquekey2 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename = (Uniquekey2 + v.KYCFile.FileName);
                        var imagePath = Path.Combine(uploadDir, filename);
                        var imageUrl = Path.Combine(uploadDir, filename);
                        v.KYCFile.SaveAs(imagePath);


                        //  Session["imagdatatavalue"] = null;
                        //Session["imagdatatavalue"] = "http://14.142.234.97/KYCDocument/" + filename;

                       // ViewBag.src = "http://14.142.234.97/KYCDocument/" + filename;

                        ViewBag.src = "https://easetours.in/kycdata/KYCDocument/" + filename;
                    
                        //v.urldatavalue = "http://14.142.234.97/KYCDocument/" + filename;


                        //  ViewBag.src = "file://14.142.234.97///Sharefolder-GIHOSP/KYCFileUpload/" + filename;
                        // ViewBag.src = " file://10.100.1.30///Sharefolder-GIHOSP/KYCFileUpload/" + filename;

                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";

                        string postData = "imgfile=" + filename;
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        v = JsonConvert.DeserializeObject<KycFileModel>(pageContent);

                        string s = JsonConvert.SerializeObject(v);
                        ErrorLog.SMTTrace("Kycdocumentdata KYCFILERESONSE doc" + s);

                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;


                    }


                    return View("Kycdocument", v);
                }
                else
                {
                    var uploadDir = ConfigurationManager.AppSettings["KYCPath"];
                    if (v.KYCFiledata != null && v.KYCFiledata.ContentLength > 0)
                    {
                        string strlocalPath = uploadDir;
                        if (!System.IO.Directory.Exists(strlocalPath))
                        {

                            System.IO.Directory.CreateDirectory(strlocalPath);
                        }
                        string extension = Path.GetExtension(v.KYCFiledata.FileName);
                        string Uniquekey3 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename = (Uniquekey3 + v.KYCFiledata.FileName);
                        var imagePath = Path.Combine(uploadDir, filename);
                        var imageUrl = Path.Combine(uploadDir, filename);
                        v.KYCFiledata.SaveAs(imagePath);




                        ViewBag.src = "https://easetours.in/kycdata/KYCDocument/" + filename;
                        //ViewBag.src = "http://14.142.234.97/KYCDocument/" + filename;
                        // v.urldatavalue = "http://14.142.234.97/KYCDocument/" + filename;




                        //ViewBag.src = "file://14.142.234.97///Sharefolder-GIHOSP/KYCFileUpload/" + filename;
                        string postData = "imgfile=" + filename;
                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        v = JsonConvert.DeserializeObject<KycFileModel>(pageContent);
                        string s = JsonConvert.SerializeObject(v);
                        ErrorLog.SMTTrace("Kycdocumentdata KYCFILEDATAdataRESONSE doc" + s);

                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;


                    }

                    //obj.DocumentProofPath = imageUrl;


                }

            }

            catch (Exception ex)
            {

                ErrorLog.SMTTrace("KYC Kycdocumentdataexception" + ex.Message);
            }
            return View("Kycdocument", v);
        }



        public ActionResult Formfilling()
        {
            FormfillingModel obj = new FormfillingModel();
            obj.fNumber = 0;
            ViewBag.filepath = "";
            return View(obj);
        }

        [HttpPost]
        public ActionResult Formfillingdata(FormfillingModel obj)
        {
            try
            {
                if (obj.FaceVerifiy != null && obj.FaceVerifiy2 != null)
                {
                    var uploadDir = ConfigurationManager.AppSettings["FormFillingfiles"];
                    if (obj.FaceVerifiy != null && obj.FaceVerifiy.ContentLength > 0)
                    {
                        string strlocalPath = uploadDir;
                        if (!System.IO.Directory.Exists(strlocalPath))
                        {

                            System.IO.Directory.CreateDirectory(strlocalPath);
                        }
                        string extension = Path.GetExtension(obj.FaceVerifiy.FileName);
                        string Uniquekey4 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename = (Uniquekey4 + obj.FaceVerifiy.FileName);
                        var imagePath = Path.Combine(uploadDir, filename);
                        //var imageUrl = Path.Combine(uploadDir, filename);
                        obj.FaceVerifiy.SaveAs(imagePath);

                        string extension2 = Path.GetExtension(obj.FaceVerifiy2.FileName);
                        string Uniquekey5 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename2 = (Uniquekey5 + obj.FaceVerifiy2.FileName);
                        var imagePath2 = Path.Combine(uploadDir, filename2);
                        //var imageUrl = Path.Combine(uploadDir, filename);
                        obj.FaceVerifiy2.SaveAs(imagePath2);
                        obj.Bankname="kvb";
                        string postData = "imgfile1=" + filename + "&imgfile2=" + filename2 + "&bankname=" +obj.Bankname;

                        //string postData = "imgfile1=01252018124311715INBHO039005234_id_1.jpg&imgfile2=01252018124311715INBHO039005234_id_1.jpg&bankname=kvb";
                       
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction_form_filling");
                      
                        // HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);
                        obj.fNumber = 1;

                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;

                        //pageContent = "{\"Message\": \"Images are matching. Confidence score is 1.0\" }";

                        //pageContent = pageContent.Replace("{ \"Message\": \"","");
                        //pageContent = pageContent.Replace("\" }", "");

                        // obj.score = pageContent;


                        //ViewBag.filepath ="http://14.142.234.97/formfillingdoc/" + obj.PdfFormFname;
                       

                       // ErrorLog.SMTTrace("obj.score" + obj.Message.ToString());



                    }
                    //obj.DocumentProofPath = imageUrl;
                    // return View("FaceVerification", obj);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.SMTTrace("Formfilling" + ex.Message);
            }


            return View("Formfilling", obj);
        }


        [HttpPost]
        public ActionResult Editformfillingdata(FormfillingModel obj)
        {
            try
            {

                string updatedjson = "";

                // updatedjson = "{ \"Name\":\"" + obj.Name + "\",\"DateofBirth\":\"" + obj.DateofBirth + "\",\"Gender\":\"" + obj.Gender + "\",\"PdfFormFname\":\"" + obj.PdfFormFname + "\",\"AddressProofNo\":\"" + obj.AddressProofNo + "\",\"FatherName\":\"" + obj.FatherName + "\",\"IdProofType\":\"" + obj.IdProofType + "\",\"PhNo\":\"" + obj.PhNo + "\",\"AddressProofType\":\"" + obj.AddressProofType + "\",\"IdProofNo\":\"" + obj.IdProofNo +"\" }";
              //  updatedjson = "{ \"Name\":\"" + obj.Name + " \",\"DateofBirth\":\"" + obj.DateofBirth + " \"Gender\":\"" + obj.Gender + "\"PdfFormFname\":\"" + obj.PdfFormFname + "\"AddressProofNo\":\"" + obj.AddressProofNo + "\"FatherName\":\"" + obj.FatherName + "\"Address\":\"" + obj.Address + "\"IdProofType\":\"" + obj.IdProofType + "\"PhNo\":\"" + obj.PhNo + "\"AddressProofType\":\"" + obj.AddressProofType + "\"IdProofNo\":\"" + obj.IdProofNo + "}";
               
                //updatedjson : {"friendid":"1","friendname":"Ashish Kalla","friendplace":"Malad","friendmobile":"777777777"};
                obj.Bankname = "kvb";
                //obj.Bankname = Session["BankName"].ToString();
                updatedjson = JsonConvert.SerializeObject(obj);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/send_json_form_filling");
                myHttpWebRequest.Method = "POST";
                ErrorLog.SMTTrace("Editformfillingdatarequest" + updatedjson.ToString());
                byte[] data = Encoding.ASCII.GetBytes(updatedjson.ToString());
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = data.Length;
                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                string pageContent = myStreamReader.ReadToEnd();
                ErrorLog.SMTTrace("EditformfillingdataResponse:" + pageContent);
                obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);
                obj.fNumber = 2;

                
                        //// string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                        //HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction_form_filling");

                        //// HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
                        //myHttpWebRequest.Method = "POST";

                        //byte[] data = Encoding.ASCII.GetBytes(postData);

                        //myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        //myHttpWebRequest.ContentLength = data.Length;

                        //Stream requestStream = myHttpWebRequest.GetRequestStream();
                        //requestStream.Write(data, 0, data.Length);
                        //requestStream.Close();

                        //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        //Stream responseStream = myHttpWebResponse.GetResponseStream();

                        //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        //string pageContent = myStreamReader.ReadToEnd();

                        //obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);

                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;

                        //pageContent = "{\"Message\": \"Images are matching. Confidence score is 1.0\" }";

                        //pageContent = pageContent.Replace("{ \"Message\": \"","");
                        //pageContent = pageContent.Replace("\" }", "");

                        // obj.score = pageContent;



                ViewBag.filepath = obj.PdfFormFname;
                ViewBag.emptypathfile = obj.EmptyPdfFormFname;


                        // ErrorLog.SMTTrace("obj.score" + obj.Message.ToString());



                    
                    //obj.DocumentProofPath = imageUrl;
                    // return View("FaceVerification", obj);
                
            }
            catch (Exception ex)
            {
                ErrorLog.SMTTrace("Editformfillingdataexception" + ex.Message);
            }


            return View("Formfilling", obj);
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FaceVerification()
        {
            KycFileModel obj = new KycFileModel();
            return View(obj);
        }

        public ActionResult FaceVerificationData(KycFileModel obj)
        {
            try
            {
                if (obj.FaceVerifiy != null && obj.FaceVerifiy2 != null)
                {
                    var uploadDir = ConfigurationManager.AppSettings["KYCPath"];
                    if (obj.FaceVerifiy != null && obj.FaceVerifiy.ContentLength > 0)
                    {
                        string strlocalPath = uploadDir;
                        if (!System.IO.Directory.Exists(strlocalPath))
                        {
 
                            System.IO.Directory.CreateDirectory(strlocalPath);
                        }
                        string extension = Path.GetExtension(obj.FaceVerifiy.FileName);
                        string Uniquekey4 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename = (Uniquekey4 + obj.FaceVerifiy.FileName);
                        var imagePath = Path.Combine(uploadDir, filename);
                        //var imageUrl = Path.Combine(uploadDir, filename);
                        obj.FaceVerifiy.SaveAs(imagePath);

                        string extension2 = Path.GetExtension(obj.FaceVerifiy2.FileName);
                        string Uniquekey5 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                        string filename2 = (Uniquekey5 + obj.FaceVerifiy2.FileName);
                        var imagePath2 = Path.Combine(uploadDir, filename2);
                        //var imageUrl = Path.Combine(uploadDir, filename);
                        obj.FaceVerifiy2.SaveAs(imagePath2);



                        ViewBag.img1 = "https://easetours.in/kycdata/KYCDocument/" + filename;
                        ViewBag.img2 = "https://easetours.in/kycdata/KYCDocument/" + filename2;
                      //  ViewBag.img1 = "http://14.142.234.97/KYCDocument/" + filename;
                       // ViewBag.img2 = "http://14.142.234.97/KYCDocument/" + filename2;
                        string postData = "imgfile1=" + filename + "&imgfile2=" + filename2;
                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4001/extraction");
                        // HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        obj = JsonConvert.DeserializeObject<KycFileModel>(pageContent);

                        //write text file
                        //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;

                        //pageContent = "{\"Message\": \"Images are matching. Confidence score is 1.0\" }";

                        //pageContent = pageContent.Replace("{ \"Message\": \"","");
                        //pageContent = pageContent.Replace("\" }", "");
                       
                       // obj.score = pageContent;

                     


                        ErrorLog.SMTTrace("obj.score" + obj.Message.ToString());



                    }
                    //obj.DocumentProofPath = imageUrl;
                   // return View("FaceVerification", obj);
                }
            }
            catch(Exception ex)
            {
                ErrorLog.SMTTrace("FaceVerificationData" + ex.Message);
            }


            return View("FaceVerification", obj);
        }


        public ActionResult Facerealtimeverifier()
        {
            return View();
        }
     
        public ActionResult Realtimeverifier()
        {
            return View();
        }

        public ActionResult FaceRealTimeVerifierData(string base64image)
        {
            KycFileModel obj = new KycFileModel();
            try
            {




                //var ImgFile = Request.Files["image"];
                //var ImgFile1 = Request.Files["image1"];
                //var uploadDir = ConfigurationManager.AppSettings["KYCPath"];

                //ErrorLog.SMTTrace("Img1");
                //ErrorLog.SMTTrace("Img2");
                //if (ImgFile != null && ImgFile1 != null)
                //{
                //    string strlocalPath = uploadDir;

                //    //convert base64 to image
                //    string filename = "";
                //    string filepath = "";
                //    string filename1 = "";
                //    if (!System.IO.Directory.Exists(strlocalPath))
                //    {

                //        System.IO.Directory.CreateDirectory(strlocalPath);
                //    }
                //    string extension = Path.GetExtension(ImgFile.ContentType);
                //    string Uniquekey3 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                //    filename = (Uniquekey3 + ".jpg");


                //    var imagePath = Path.Combine(uploadDir, filename);
                //    //  var imageUrl = Path.Combine(uploadDir, filename);
                //    ErrorLog.SMTTrace("Img1------" + filename);
                //    ImgFile.SaveAs(imagePath);

                //    string extension1 = Path.GetExtension(ImgFile1.ContentType);
                //    string Uniquekey1 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                //    filename1 = (Uniquekey1 + ".jpg");

                //    var imagePath1 = Path.Combine(uploadDir, filename1);
                //    //  var imageUrl1 = Path.Combine(uploadDir, filename1);
                //    ErrorLog.SMTTrace("Img2-------" + filename1);
                //    ImgFile1.SaveAs(imagePath1);


                    try
                    {


                        //KYC extraction
                        //string postData = "imgfile=" + filename;
                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                        string imgstring = "";
                        imgstring = base64image.ToString();

                        imgstring  = "{\"imgstring\":\"" + imgstring + "\"}";



                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4010/age_detection");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(imgstring);

                        myHttpWebRequest.ContentType = "application/json";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        
                        string pageContent = myStreamReader.ReadToEnd();
                        ErrorLog.SMTTrace("FaceRealTimeVerifierDataResponse:" + pageContent);

                        obj = JsonConvert.DeserializeObject<KycFileModel>(pageContent);
                        string s = JsonConvert.SerializeObject(obj);
                        ErrorLog.SMTTrace("FaceRealTimeVerifierData" + s);
                    }
                    catch (Exception ex)
                    {

                        ErrorLog.SMTTrace("FaceRealTimeVerifierDataexception" + ex.Message);
                    }

                   

                }


                //obj.DocumentProofPath = imageUrl;

            

            catch (Exception ex)
            {

                ErrorLog.SMTTrace(ex.Message);

            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult RealTimeVerifierData()
        //{
        //    KycFileModel obj = new KycFileModel();
        //    try
        //    {




        //        var ImgFile = Request.Files["image"];
        //        var ImgFile1 = Request.Files["image1"];
        //        var uploadDir = ConfigurationManager.AppSettings["KYCPath"];

        //        ErrorLog.SMTTrace("Img1");
        //        ErrorLog.SMTTrace("Img2");
        //        if (ImgFile != null && ImgFile1 != null)
        //        {
        //            string strlocalPath = uploadDir;

        //            //convert base64 to image
        //            string filename = "";
        //            string filepath = "";
        //            string filename1 = "";
        //            if (!System.IO.Directory.Exists(strlocalPath))
        //            {

        //                System.IO.Directory.CreateDirectory(strlocalPath);
        //            }
        //            string extension = Path.GetExtension(ImgFile.ContentType);
        //            string Uniquekey3 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

        //            filename = (Uniquekey3 + ".jpg");


        //            var imagePath = Path.Combine(uploadDir, filename);
        //            //  var imageUrl = Path.Combine(uploadDir, filename);
        //            ErrorLog.SMTTrace("Img1------" + filename);
        //            ImgFile.SaveAs(imagePath);

        //            string extension1 = Path.GetExtension(ImgFile1.ContentType);
        //            string Uniquekey1 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

        //            filename1 = (Uniquekey1 + ".jpg");

        //            var imagePath1 = Path.Combine(uploadDir, filename1);
        //            //  var imageUrl1 = Path.Combine(uploadDir, filename1);
        //            ErrorLog.SMTTrace("Img2-------" + filename1);
        //            ImgFile1.SaveAs(imagePath1);


        //            try
        //            {


        //                //KYC extraction
        //                string postData = "imgfile=" + filename;
        //                // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
        //                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://115.110.173.83:4003/extraction");
        //                myHttpWebRequest.Method = "POST";

        //                byte[] data = Encoding.ASCII.GetBytes(postData);

        //                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //                myHttpWebRequest.ContentLength = data.Length;

        //                Stream requestStream = myHttpWebRequest.GetRequestStream();
        //                requestStream.Write(data, 0, data.Length);
        //                requestStream.Close();

        //                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

        //                Stream responseStream = myHttpWebResponse.GetResponseStream();

        //                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

        //                string pageContent = myStreamReader.ReadToEnd();

        //                obj = JsonConvert.DeserializeObject<KycFileModel>(pageContent);
        //                string s = JsonConvert.SerializeObject(obj);
        //                ErrorLog.SMTTrace("KYC doc" + s);
        //            }
        //            catch (Exception ex)
        //            {

        //                ErrorLog.SMTTrace("KYC extraction" + ex.Message);
        //            }

        //            try
        //            {



        //                //KYC Fave Verification

        //                string postData1 = "imgfile1=" + filename + "&imgfile2=" + filename1;
        //                // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
        //                HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)HttpWebRequest.Create("http://115.110.173.83:4001/extraction");
        //                // HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
        //                myHttpWebRequest1.Method = "POST";

        //                byte[] data1 = Encoding.ASCII.GetBytes(postData1);

        //                myHttpWebRequest1.ContentType = "application/x-www-form-urlencoded";
        //                myHttpWebRequest1.ContentLength = data1.Length;

        //                Stream requestStream1 = myHttpWebRequest1.GetRequestStream();
        //                requestStream1.Write(data1, 0, data1.Length);
        //                requestStream1.Close();

        //                HttpWebResponse myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();

        //                Stream responseStream1 = myHttpWebResponse1.GetResponseStream();

        //                StreamReader myStreamReader1 = new StreamReader(responseStream1, Encoding.Default);

        //                string pageContent1 = myStreamReader1.ReadToEnd();


        //                obj.score = pageContent1;

        //                ErrorLog.SMTTrace("KYC score" + obj.score);
        //            }
        //            catch (Exception ex)
        //            {

        //                ErrorLog.SMTTrace("KYC Fave Verification" + ex.Message);
        //            }

        //        }


        //        //obj.DocumentProofPath = imageUrl;

        //    }

        //    catch (Exception ex)
        //    {

        //        ErrorLog.SMTTrace(ex.Message);

        //    }
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Realtimedata()
       {
            FormfillingModel obj = new FormfillingModel();
            obj.fNumber = 0;
            return View(obj);
        }


        public ActionResult Realtimefillingdata()
        {
            FormfillingModel obj = new FormfillingModel();
            FormfillingModel objReal = new FormfillingModel();
            try
            {




                var ImgFile = Request.Files["image"];
                var ImgFile1 = Request.Files["image1"];
                var ImgFile2 = Request.Files["image2"];
                var uploadDir = ConfigurationManager.AppSettings["KYCPath"];
                var uploadDir1 = ConfigurationManager.AppSettings["FormFillingfiles"];
                ErrorLog.SMTTrace("Img1");
                ErrorLog.SMTTrace("Img2");
                if (ImgFile != null && ImgFile1 != null)
                {
                    string strlocalPath = uploadDir;
                    string strlocalPath1 = uploadDir1;
                    string filename = "";
                    string filename1 = "";
                    string AddressProoffilename = "";
                    if (!System.IO.Directory.Exists(strlocalPath))
                    {

                        System.IO.Directory.CreateDirectory(strlocalPath);
                    }
                    if (!System.IO.Directory.Exists(strlocalPath1))
                    {

                        System.IO.Directory.CreateDirectory(strlocalPath1);
                    }
                    string extension = Path.GetExtension(ImgFile.ContentType);
                    string Uniquekey3 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                    filename = (Uniquekey3 + ".jpg");

                    ErrorLog.SMTTrace("Realtimefillingdataextraction---" + filename.ToString());
                    var imagePath = Path.Combine(uploadDir, filename);
                    var FormfillingPath = Path.Combine(uploadDir1, filename);
                    //  var imageUrl = Path.Combine(uploadDir, filename);

                    ImgFile.SaveAs(imagePath);
                    ImgFile.SaveAs(FormfillingPath);

                    string extension1 = Path.GetExtension(ImgFile1.ContentType);
                    string Uniquekey1 = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                    filename1 = (Uniquekey1 + ".jpg");

                    ErrorLog.SMTTrace("Realtimefillingdataextraction---" + filename1.ToString());
                    var imagePath1 = Path.Combine(uploadDir, filename1);


                    string Imageextension = Path.GetExtension(ImgFile2.ContentType);
                    string ImageUniquekey = DateTime.Now.ToString("MMddyyyyHHmmssfff");

                    AddressProoffilename = (ImageUniquekey + ".jpg");

                    ErrorLog.SMTTrace("Realtimefillingdataextraction---" + AddressProoffilename.ToString());
                    var AddressProofPath = Path.Combine(uploadDir1, AddressProoffilename);
                    //  var imageUrl1 = Path.Combine(uploadDir, filename1);
                    ImgFile2.SaveAs(AddressProofPath);
                    try
                    {


                        //KYC extraction
                        string postData = "imgfile=" + filename;
                        //string postData = "imgfile=01252018110301699.jpg";
                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction");
                        myHttpWebRequest.Method = "POST";

                        byte[] data = Encoding.ASCII.GetBytes(postData);

                        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest.ContentLength = data.Length;

                        Stream requestStream = myHttpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                        Stream responseStream = myHttpWebResponse.GetResponseStream();

                        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                        string pageContent = myStreamReader.ReadToEnd();

                        obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);
                        //if (obj.score == "More than one face found" || obj.score == "Owner Image Face detection failed, Upload better image / Upload image with face" || obj.score == "KYC document Image Face detection failed, Upload better image / Upload image with face" || obj.score == "Images are not matching")
                        //{ obj.score = "Face verification failed."; }
                        //else { obj.score = "Face verified sucessfully."; }
                        //objReal.score = obj.Message;
                        if (obj.Message != "")
                        {
                            objReal.score = obj.score;
                        }
                        else
                        {
                            objReal.score = obj.Message;
                        }
                        string s = JsonConvert.SerializeObject(obj);
                        ErrorLog.SMTTrace("Realtimefillingdata" + s);
                        
                    }
                    catch (Exception ex)
                    {

                        ErrorLog.SMTTrace("Realtimefillingdata" + ex.Message);
                    }

                    try
                    {



                        //KYC Fave Verification

                        ErrorLog.SMTTrace("RealtimefillingdataformfillingImage1---" + filename.ToString());

                        ErrorLog.SMTTrace("RealtimefillingdataformfillingAddressProoffilenameImage---" + AddressProoffilename.ToString());
                       
                        obj.Bankname = "kvb";
                        string postData1 = "imgfile1=" + filename + "&imgfile2=" + AddressProoffilename + "&bankname=" + obj.Bankname;
                        Session["Realtimedatabank"] = obj.Bankname;
                        //string postData1 = "imgfile1=01252018124311715INBHO039005234_id_1.jpg&imgfile2=01252018124311715INBHO039005234_id_1.jpg&bankname=kvb";
                        HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/extraction_form_filling");
                       // string postData1 = "imgfile1=" + filename + "&imgfile2=" + filename1;
                        // string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                      //  HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4001/extraction");
                        // HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
                        myHttpWebRequest1.Method = "POST";

                        byte[] data1 = Encoding.ASCII.GetBytes(postData1);

                        myHttpWebRequest1.ContentType = "application/x-www-form-urlencoded";
                        myHttpWebRequest1.ContentLength = data1.Length;

                        Stream requestStream1 = myHttpWebRequest1.GetRequestStream();
                        requestStream1.Write(data1, 0, data1.Length);
                        requestStream1.Close();

                        HttpWebResponse myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();

                        Stream responseStream1 = myHttpWebResponse1.GetResponseStream();

                        StreamReader myStreamReader1 = new StreamReader(responseStream1, Encoding.Default);

                        string pageContent1 = myStreamReader1.ReadToEnd();

                        objReal = JsonConvert.DeserializeObject<FormfillingModel>(pageContent1);
                        objReal.fNumber = 1;
                        //objReal.score = obj.Message;
                        //if (objReal.Message != "")
                        //{
                        //    objReal.score = obj.score;
                        //}
                        //else
                        //{
                        //    objReal.score = obj.Message;
                        //}


                    

                        ErrorLog.SMTTrace("Realtimefillingdatascore" + obj.score);
                    }
                    catch (Exception ex)
                    {

                        ErrorLog.SMTTrace("Realtimefillingdata" + ex.Message);
                    }

                }


                //obj.DocumentProofPath = imageUrl;

            }

            catch (Exception ex)
            {

                ErrorLog.SMTTrace(ex.Message);

            }
            return Json(objReal, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Realtimedata(FormfillingModel obj)
        {
            try
            {

                string updatedjson = "";

                // updatedjson = "{ \"Name\":\"" + obj.Name + "\",\"DateofBirth\":\"" + obj.DateofBirth + "\",\"Gender\":\"" + obj.Gender + "\",\"PdfFormFname\":\"" + obj.PdfFormFname + "\",\"AddressProofNo\":\"" + obj.AddressProofNo + "\",\"FatherName\":\"" + obj.FatherName + "\",\"IdProofType\":\"" + obj.IdProofType + "\",\"PhNo\":\"" + obj.PhNo + "\",\"AddressProofType\":\"" + obj.AddressProofType + "\",\"IdProofNo\":\"" + obj.IdProofNo +"\" }";
                //  updatedjson = "{ \"Name\":\"" + obj.Name + " \",\"DateofBirth\":\"" + obj.DateofBirth + " \"Gender\":\"" + obj.Gender + "\"PdfFormFname\":\"" + obj.PdfFormFname + "\"AddressProofNo\":\"" + obj.AddressProofNo + "\"FatherName\":\"" + obj.FatherName + "\"Address\":\"" + obj.Address + "\"IdProofType\":\"" + obj.IdProofType + "\"PhNo\":\"" + obj.PhNo + "\"AddressProofType\":\"" + obj.AddressProofType + "\"IdProofNo\":\"" + obj.IdProofNo + "}";

                //updatedjson : {"friendid":"1","friendname":"Ashish Kalla","friendplace":"Malad","friendmobile":"777777777"};
                obj.Bankname ="kvb";
                updatedjson = JsonConvert.SerializeObject(obj);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://220.225.104.138:4003/send_json_form_filling");
                myHttpWebRequest.Method = "POST";
                ErrorLog.SMTTrace("Realtimedatarequest" + updatedjson.ToString());
                byte[] data = Encoding.ASCII.GetBytes(updatedjson.ToString());
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.ContentLength = data.Length;
                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                string pageContent = myStreamReader.ReadToEnd();
                ErrorLog.SMTTrace("RealtimedataResponse:" + pageContent);
                obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);
                obj.fNumber = 2;


                //// string postData = "imgfile=INBDQ03900002F_id_1.jpg";
                //HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://115.110.173.83:4003/extraction_form_filling");

                //// HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://10.100.1.245:4001/extraction");
                //myHttpWebRequest.Method = "POST";

                //byte[] data = Encoding.ASCII.GetBytes(postData);

                //myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //myHttpWebRequest.ContentLength = data.Length;

                //Stream requestStream = myHttpWebRequest.GetRequestStream();
                //requestStream.Write(data, 0, data.Length);
                //requestStream.Close();

                //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //Stream responseStream = myHttpWebResponse.GetResponseStream();

                //StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                //string pageContent = myStreamReader.ReadToEnd();

                //obj = JsonConvert.DeserializeObject<FormfillingModel>(pageContent);

                //write text file
                //string Data = AgentId + "," + pan.Name + "," + pan.DateofBirth + "," + pan.FatherName + "," + pan.PANNumber;

                //pageContent = "{\"Message\": \"Images are matching. Confidence score is 1.0\" }";

                //pageContent = pageContent.Replace("{ \"Message\": \"","");
                //pageContent = pageContent.Replace("\" }", "");

                // obj.score = pageContent;



              //  ErrorLog.SMTTrace("Realtimefillingdatafile" + obj.PdfFormFname);

              //  ErrorLog.SMTTrace("Realtimefillingdataempthyfile" + obj.EmptyPdfFormFname);



               // ViewBag.filepathRealtimedata = "https://easetours.in/KYCdata/FormFillingfiles/" + obj.PdfFormFname;
               // ViewBag.emptypathfileRealtimedata = "https://easetours.in/KYCdata/FormFillingfiles/" + obj.EmptyPdfFormFname;

                ViewBag.filepathRealtimedata =  obj.PdfFormFname;
                ViewBag.emptypathfileRealtimedata =  obj.EmptyPdfFormFname;


                // ErrorLog.SMTTrace("obj.score" + obj.Message.ToString());




                //obj.DocumentProofPath = imageUrl;
                // return View("FaceVerification", obj);

            }
            catch (Exception ex)
            {
                ErrorLog.SMTTrace("Realtimedataexception" + ex.Message);
            }


            return View("Realtimedata", obj);
        }


        public ActionResult Pdfoutputfile()
        {
            FormfillingModel obj = new FormfillingModel();
            return View(obj);
        }

        public ActionResult Pdfoutputfiledata(FormfillingModel obj)
        {

            try
            {

                DLayer dlayer = new DLayer();
                string strError = "";


                ErrorLog.SMTTrace("Pdfoutputfiledatabefore Request username:" + obj.MobileNumber);
                DataSet ds = dlayer.Qry_WithDataset("CLI_GET_KycImageFiles'" + obj.MobileNumber +"'",ref strError);

                 obj.lstfinalpdfoutfile = (from DataRow row in ds.Tables[0].AsEnumerable()
                                   select new finalpdfoutfiledata()
                                   {
                                     
                                       Filename1 = row["FileName1"].ToString(),
                                       Filename2 = row["FileName2"].ToString()
                                     
                                   }).ToList();
            }
            catch(Exception ex)
            {
                ErrorLog.Write(ex.Message, "HomeController", "Index");
            }
            
            return View("Pdfoutputfile", obj);
            }


        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public ActionResult DocumentViewFile(string FileName)
        {

            var uploadDir = ConfigurationManager.AppSettings["KYCPath"];

                 string  FilePath = uploadDir + FileName;

            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            return File(fs, GetMimeType(FilePath));
        }

        public ActionResult Cibildata()
        {
            return View();
        }


        }

   


        //ASCII
        //public string Realsampledatavlue()
        //{
        //    string strresult = "";
        //    string strresult1 = "";
        //    int answer = 0;
        //    int answer1 = 0;
        //    string value = "";
        //    string strresult2 = "";
        //    string dt = "";
        //    int b = 0;
        //    const string input = "Venkat";
        //    byte[] array = Encoding.ASCII.GetBytes(input);


        //    foreach (byte element in array)
        //    {

        //        dt = DateTime.Now.ToString("dd");




        //        b = Convert.ToInt32(dt);

        //        strresult = Convert.ToString(element);



        //        if (strresult.Length == 2)
        //        {

        //            strresult1 = Convert.ToString(element);
        //            answer = Convert.ToInt32(strresult) + b;

        //            strresult2 = answer.ToString();
        //        }
        //        else
        //        {
        //            answer = element + b;
        //            strresult2 = answer.ToString();
        //        }


        //        //answer1 = 36 + b;
        //        //value += strresult2 + Convert.ToString(answer1);

        //        // answer1 = 36 + b;
        //        value += strresult2 + "~";




        //    }

        //    return value;

        //}



        
        //public void Realsampledatavluemadhu()
        //{

        //    System.Web.HttpContext.Current.Response.Clear();
        //    System.Web.HttpContext.Current.Response.Charset = "";

        //    System.Web.HttpContext.Current.Response.ContentType = "application/msword";

        //    string strFileName = "GenerateDocument" + ".doc";
        //    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);

        //    StringBuilder strHTMLContent = new StringBuilder();

        //    strHTMLContent.Append(" <h1 title='Heading' align='Center' style='font-family:verdana;font-size:80%;color:black'><u>Document Heading</u> </h1>".ToString());
        //    strHTMLContent.Append("<br>".ToString());
        //    strHTMLContent.Append("<table align='Center'>".ToString());
        //    strHTMLContent.Append("<tr>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px;background:#99CC00'><b>Column 1</b></td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px;background:#99CC00'><b>Column 2</b></td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px;background:#99CC00'><b>Column 3</b></td>".ToString());
        //    strHTMLContent.Append("</tr>".ToString());
        //    strHTMLContent.Append("<tr>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>a</td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>b</td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>c</td>".ToString());
        //    strHTMLContent.Append("</tr>".ToString());
        //    strHTMLContent.Append("<tr>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>d</td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>e</td>".ToString());
        //    strHTMLContent.Append("<td style='width: 100px'>f</td>".ToString());
        //    strHTMLContent.Append("</tr>".ToString());
        //    strHTMLContent.Append("</table>".ToString());
        //    strHTMLContent.Append("<br><br>".ToString());
        //    strHTMLContent.Append("<p align='Center'> Note : This is dynamically generated word document </p>".ToString());


        //    System.Web.HttpContext.Current.Response.Write(strHTMLContent);
        //    System.Web.HttpContext.Current.Response.End();
        //    System.Web.HttpContext.Current.Response.Flush();

        //}

    }



dropdown items:

        public static List<SelectListItem> GetsubQueryCategory(string CategoryId)
        {
            var d = obj.GetQuerySubCategory(CategoryId);
            var list = d.Tables[0].AsEnumerable().Select(r => new SelectListItem
            {
                Text = r.Field<string>("SubCategoryDesc"),
                Value = r.Field<int>("SubCategoryId").ToString()

            }).ToList();
            return list;

        }



getbaseurl:

 public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request.Url == (Uri)null)
                return string.Empty;
            else
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }
