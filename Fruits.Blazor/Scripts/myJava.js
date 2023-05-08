/*
* MyJava
 */

async function closeModal(id) {
    var myModalEl = document.getElementById(id);
    var modal = window.bootstrap.Modal.getInstance(myModalEl); // Returns a Bootstrap modal instance
    modal.hide();
}

async function ToastShow(liveToast) {
    var toastLiveExample = document.getElementById(liveToast);
    var toast = new window.bootstrap.Toast(toastLiveExample);
    toast.show();
}