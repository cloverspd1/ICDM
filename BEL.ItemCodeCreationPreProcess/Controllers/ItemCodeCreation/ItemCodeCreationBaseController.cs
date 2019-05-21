namespace BEL.ItemCodeCreationPreProcess.Controllers.ItemCodeCreation
{
    using BEL.DataAccessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using BusinessLayer;
    using CommonDataContract;
    using Models.ItemCode;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Item Code Creation Base Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class ItemCodeCreationBaseController : BaseController
    {
        /// <summary>
        /// Gets the item code creation details.
        /// </summary>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns></returns>
        public ItemCodeContract GetItemCodeCreationDetails(Dictionary<string, string> objDict)
        {
            return ItemCodeCreationBusinessLayer.Instance.GetItemCodeCreationDetails(objDict);
        }

        /// <summary>
        /// Saves the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>return ActionStatus</returns>
        protected ActionStatus SaveSection(ISection section, Dictionary<string, string> objDict)
        {
            ActionStatus status = new ActionStatus();
            status = ItemCodeCreationBusinessLayer.Instance.SaveBySection(section, objDict);
            return status;
        }

        /// <summary>
        /// Gets the Vendor.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns>json object</returns>
        public JsonResult GetVendors(string q)
        {
            string data = ItemCodeCreationBusinessLayer.Instance.GetVendorMasterData(q);
            if (!string.IsNullOrEmpty(data))
            {
                var master = JSONHelper.ToObject<VendorMaster>(data);
                return this.Json((from item in master.Items select new { id = item.Value + " - " + item.Title, name = item.Value + " - " + item.Title, items = item }).ToList(), JsonRequestBehavior.AllowGet);
            }
            return this.Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}