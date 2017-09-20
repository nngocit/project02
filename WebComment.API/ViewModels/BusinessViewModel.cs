using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Models;
using WebComment.Data;


namespace WebComment.API.ViewModels
{
   
    public class BusinessViewModel 
    {

        public List<BusinessProductModel> BusinessProduct { get; set; }
        public List<B_Business> Business { get; set; }
        public List<string> LoaiSanpham { get; set; }
         public List<string> Thuonghieu  { get; set; }

        
      
    }
}