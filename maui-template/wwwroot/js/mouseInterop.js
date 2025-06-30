window.mouseInterop = {
    subscribeMouseMove: function (dotNetObjRef) {
        window.onmousemove = function (e) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseMove', e.screenX, e.screenY);
        };
    },
    unsubscribeMouseMove: function () {
        window.onmousemove = null;
    }
};
window.mouseDragInterop = {
    startDrag: function (dotNetObjRef) {
        function onMouseMove(e) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseMove');
        }
        function onMouseUp(e) {
            dotNetObjRef.invokeMethodAsync('OnGlobalMouseUp');
            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);
        }
        window.addEventListener('mousemove', onMouseMove);
        window.addEventListener('mouseup', onMouseUp);
    }
};
