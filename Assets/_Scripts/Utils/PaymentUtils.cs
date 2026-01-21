using System;
using Sfs2X.Entities.Data;
using UnityEngine;

public class PaymentUtils : Singleton<PaymentUtils>
{
    public event Action ChangeStatusToNLDSuccess;

    public event Action ChangePiggyBank;

    //public event Action ChangeFlashSale;
    public long maxPotPiggyBank = 50000;

    // public void GetPackageAmountSuccess(SFSObject sfsObject)
    // {
    //     var data = new PackageAmountSuccessResponseVO();
    //     data.FromSfsObject(sfsObject);
    //     DepositSuccessPopup.Open(view => view.UpdateUI(data));
    //     NLDDragableIcon.Close();
    // }
    //
    // public void GetWithDrawResponse(SFSObject sfsObject)
    // {
    //     var data = new WithDrawRequestResponseVO();
    //     data.FromSfsObject(sfsObject);
    //     WithDrawSuccessPopup.Open(view => view.UpdateUI(data));
    //     ChangeStatusToNLDSuccess?.Invoke();
    // }
    //
    // public void GetWithDrawNotification(SFSObject sfsObject)
    // {
    //     var data = new WithDrawNotificationResponseVO();
    //     data.FromSfsObject(sfsObject);
    //     WithDrawNotificationPopup.Open(view => view.UpdateUI(data));
    // }
    //
    // public void PiggyBankDepositSuccess(SFSObject sfsObject)
    // {
    //     var data = new PiggyBankSuccessResponseVO();
    //     data.FromSfsObject(sfsObject);
    //     PiggyBankDepositSuccessPopup.Open(view => view.UpdateUI(data));
    //     if (ScreenManager.Instance.IsOpenScene(SceneId.PortalScene))
    //         ChangePiggyBank?.Invoke();
    // }
    //
    // public void FlashSaleCreateSuccess(SFSObject sfsObject)
    // {
    //     var data = new FlashSaleCreateResponseVO();
    //     data.FromSfsObject(sfsObject);
    //     FlashSaleCreatePopup.Open(view => view.UpdateUI(data));
    //     // if (ScreenManager.Instance.IsOpenScene(SceneId.PortalScene))
    //     //     ChangeFlashSale?.Invoke();
    // }
}