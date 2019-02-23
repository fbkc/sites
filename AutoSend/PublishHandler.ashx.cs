using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AutoSend
{
    /// <summary>
    /// PublishHandler 的摘要说明
    /// </summary>
    public class PublishHandler : BaseHandle, IHttpHandler, IRequiresSessionState
    {

        public override void OnLoad(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(json.WriteJson(0, "禁止访问", new { }));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        #region 发布设置
                        case "getproductlist": _strContent.Append(GetProductList(context)); break;//获取此会员下所有产品分类
                        case "getproductbyid": _strContent.Append(GetProductById(context)); break;//获取某个产品具体信息
                        case "saveproduct": _strContent.Append(SaveProduct(context)); break;
                        case "delproduct": _strContent.Append(DelProduct(context)); break;//删除产品分类
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 产品/新闻
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProductList(HttpContext context)
        {
            productBLL bll = new productBLL();
            List<productInfo> pList = new List<productInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                DataTable dt = bll.GetProduct(string.Format(" where userId='{0}'", userId));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        productInfo pInfo = new productInfo();
                        pInfo.Id = (int)row["Id"];
                        pInfo.productName = (string)row["productName"];
                        pInfo.userId = (int)row["userId"];
                        if (row["pinpai"] == DBNull.Value)
                            pInfo.pinpai = null;
                        else
                            pInfo.pinpai = (string)row["pinpai"];
                        if (row["xinghao"] == DBNull.Value)
                            pInfo.xinghao = null;
                        else
                            pInfo.xinghao = (string)row["xinghao"];
                        if (row["price"] == DBNull.Value)
                            pInfo.price = null;
                        else
                            pInfo.price = (string)row["price"];
                        if (row["smallCount"] == DBNull.Value)
                            pInfo.smallCount = null;
                        else
                            pInfo.smallCount = (string)row["smallCount"];
                        if (row["sumCount"] == DBNull.Value)
                            pInfo.sumCount = null;
                        else
                            pInfo.sumCount = (string)row["sumCount"];
                        if (row["unit"] == DBNull.Value)
                            pInfo.unit = null;
                        else
                            pInfo.unit = (string)row["unit"];
                        if (row["city"] == DBNull.Value)
                            pInfo.city = null;
                        else
                            pInfo.city = (string)row["city"];
                        pInfo.createTime = ((DateTime)row["createTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        pInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        if (row["informationType"] == DBNull.Value)
                            pInfo.informationType = null;
                        else
                            pInfo.informationType = (string)row["informationType"];//产品/新闻
                        if (row["maxPubCount"] == DBNull.Value)
                            pInfo.maxPubCount = null;
                        else
                            pInfo.maxPubCount = (int)row["maxPubCount"];
                        if (row["endPubCount"] == DBNull.Value)
                            pInfo.endPubCount = null;
                        else
                            pInfo.endPubCount = (int)row["endPubCount"];
                        if (row["endTodayPubCount"] == DBNull.Value)
                            pInfo.endTodayPubCount = null;
                        else
                            pInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                        if (row["pub_startTime"] == DBNull.Value)
                            pInfo.pub_startTime = null;
                        else
                            pInfo.pub_startTime = ((DateTime)row["pub_startTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        if (row["pubInterval"] == DBNull.Value)
                            pInfo.pubInterval = null;
                        else
                            pInfo.pubInterval = (int)row["pubInterval"];
                        pInfo.isPub = (bool)row["isPub"];
                        pInfo.isdel = (bool)row["isdel"];
                        pList.Add(pInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { productList = pList });
        }

        /// <summary>
        /// 获取单个产品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProductById(HttpContext context)
        {
            string productId = context.Request["productId"];
            productBLL bll = new productBLL();
            productInfo pInfo = new productInfo();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                DataTable dt = bll.GetProduct(string.Format(" where userId='{0}' and Id='{1}'", userId, productId));
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    pInfo.Id = (int)row["Id"];
                    pInfo.productName = (string)row["productName"];
                    pInfo.userId = (int)row["userId"];
                    if (row["pinpai"] == DBNull.Value)
                        pInfo.pinpai = null;
                    else
                        pInfo.pinpai = (string)row["pinpai"];
                    if (row["xinghao"] == DBNull.Value)
                        pInfo.xinghao = null;
                    else
                        pInfo.xinghao = (string)row["xinghao"];
                    if (row["price"] == DBNull.Value)
                        pInfo.price = null;
                    else
                        pInfo.price = (string)row["price"];
                    if (row["smallCount"] == DBNull.Value)
                        pInfo.smallCount = null;
                    else
                        pInfo.smallCount = (string)row["smallCount"];
                    if (row["sumCount"] == DBNull.Value)
                        pInfo.sumCount = null;
                    else
                        pInfo.sumCount = (string)row["sumCount"];
                    if (row["unit"] == DBNull.Value)
                        pInfo.unit = null;
                    else
                        pInfo.unit = (string)row["unit"];
                    if (row["city"] == DBNull.Value)
                        pInfo.city = null;
                    else
                        pInfo.city = (string)row["city"];
                    pInfo.createTime = ((DateTime)row["createTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    pInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    if (row["informationType"] == DBNull.Value)
                        pInfo.informationType = null;
                    else
                        pInfo.informationType = (string)row["informationType"];//产品/新闻
                    if (row["maxPubCount"] == DBNull.Value)
                        pInfo.maxPubCount = null;
                    else
                        pInfo.maxPubCount = (int)row["maxPubCount"];
                    if (row["endPubCount"] == DBNull.Value)
                        pInfo.endPubCount = null;
                    else
                        pInfo.endPubCount = (int)row["endPubCount"];
                    if (row["endTodayPubCount"] == DBNull.Value)
                        pInfo.endTodayPubCount = null;
                    else
                        pInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                    if (row["pub_startTime"] == DBNull.Value)
                        pInfo.pub_startTime = null;
                    else
                        pInfo.pub_startTime = ((DateTime)row["pub_startTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    if (row["pubInterval"] == DBNull.Value)
                        pInfo.pubInterval = null;
                    else
                        pInfo.pubInterval = (int)row["pubInterval"];
                    pInfo.isPub = (bool)row["isPub"];
                    pInfo.isdel = (bool)row["isdel"];
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { product = pInfo });
        }
        /// <summary>
        /// 增加或修改产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveProduct(HttpContext context)
        {
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                productBLL bll = new productBLL();
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                productInfo product = JsonConvert.DeserializeObject<productInfo>(strjson, js);
                product.userId = model.Id;
                if (product.Id == 0)
                    bll.AddProduct(product);
                else
                    bll.UpdateProduct(product);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelProduct(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            productBLL bll = new productBLL();
            int a = bll.DelProduct(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}