using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Qalsql.Controllers
{
    [Authorize]
    public class AuthorizedController : AsyncController
    {
        private string _uid;

        protected string UserId {
            get
            {
                if (_uid == null)
                {
                    if (User != null)
                    {
                        _uid = User.Identity.GetUserId();
                    }
                }
                return _uid;
            } 
        }
    }
}