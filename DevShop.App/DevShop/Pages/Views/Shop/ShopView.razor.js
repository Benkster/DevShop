export function CloseTreeView() {
    var chkTree = document.querySelector('#categoryTree > input#chk_toggleTree');


    if (chkTree)
    {
        chkTree.checked = false;
    }
}