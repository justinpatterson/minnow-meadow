using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elara.TileSystem;

namespace Elara.GameManagement
{
    public class TileAction : MonoBehaviour
    {

        ActionManager.ActionRequest _request;
        ActionManager _actionMgr;

        public virtual void DoAction(ActionManager.ActionRequest actionRq, ActionManager actionMgr) 
        {
            _request = actionRq;
            _actionMgr = actionMgr;
            //play an animation on _tileRef
            //increment or decrement a resource value
            //and complete
            bool isValidReq = _actionMgr.ValidateActionRequest(_request);
            if (isValidReq) 
            {
                _request.tile.TileActionHandler(_request.actionType);
            }
            CompleteAction();
        }
        protected virtual void CompleteAction() 
        {
            _actionMgr?.ReportActionComplete(_request);
        }
    }
}

