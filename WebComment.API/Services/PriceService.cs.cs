using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using WebComment.API.Models;
using WebComment.API.ViewModels;
using WebComment.Commons;

namespace WebComment.API.Services
{
    public class PriceService
    {
        public static List<PriceModel> GetGia_Api(PriceParamModel productmodel)
        {
            try
            {
                var client = new RestClient(Globals.VTAWebAPIUrl);
                var req = new RestRequest("vnpost-api/price/ListPriceByBranch", Method.POST) { RequestFormat = DataFormat.Json };
                req.AddBody(productmodel);

                var rs = client.Execute<HttpContentResult<List<PriceModel>>>(req).Data;

                if (rs.Data == null && rs.StatusCode != "200")
                    return null;
                return rs.Data;
            }
            catch (Exception ex)
            {
                var param = productmodel.ProductCodes.Aggregate(string.Empty, (current, item) => current + item + ",");
                param = param + " | " + productmodel.StoreCode;

                NLog.LogManager.GetCurrentClassLogger().Debug(" vnpost-api/price/ListPriceByBranch - Ex: - ({0}) - Param: {1}", ex.ToString(), param);
                return null;
            }

        }

        //public static List<PriceModel> GiaApi142(string CodesId, string StoreId)
        //{
        //    var client = new RestClient(Globals.VTAWebAPIPublicUrl);
        //    var req = new RestRequest("/price-api/Product?CodesId=" + CodesId + "&StoreId=" + StoreId, Method.GET) { RequestFormat = DataFormat.Json };

        //    var rs = client.Execute<HttpContentResult<List<PriceModel>>>(req).Data;

        //    if (rs.Data == null && rs.StatusCode != "200")
        //        return null;
        //    return rs.Data;
        //}
        //}
    }
}