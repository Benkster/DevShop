export function ResetPage() {
    var chkTree = document.querySelector('#categoryTree > input#chk_toggleTree');
    var radListDet = document.getElementById('rad_hideListDetails');
    var radListDetDescr = document.getElementById('rad_hideListDetDescription');


    if (chkTree)
    {
        chkTree.checked = false;
    }

    if (radListDet)
    {
        radListDet.checked = true;
    }

    if (radListDetDescr)
    {
        radListDetDescr.checked = true;
    }



    CheckCookie();
}



export function SetCookie(_cookieValue) {
    if (_cookieValue.length > 0)
    {
        var now = new Date();
        var exp = new Date(now.getTime() + (1000 * 60 * 60 * 24));

        document.cookie = 'ShopView=' + _cookieValue + '; expires=' + exp + '; SameSite=None; path=/shop';
    }
}



export function CheckCookie() {
    var shopCookie = document.cookie.match('ShopView=(.*);?');


    if (shopCookie)
    {
        var cookieVal = shopCookie[1];


        if (cookieVal == 'list') {
            var shopRadio = document.getElementById('rad_listView');


            if (shopRadio) {
                shopRadio.checked = true;
            }
        }
        else
        {
            var shopRadio = document.getElementById('rad_boxView');


            if (shopRadio) {
                shopRadio.checked = true;
            }
        }
    }
}