function LightFrame(_url, _width, _height) {
	var popupContent = '<iframe id="popupIframe" src="' + _url + '" width="' + _width + '" height="' + _height + '"></iframe>';

	OpenLightFrame(popupContent);
}



function LightFrame(_url, _width, _height, _maxWidth, _maxHeight) {
	var popupContent = '<iframe id="popupIframe" src="' + _url + '" width="' + _width + '" height="' + _height + '" style="max-width: ' + _maxWidth + '; max-height: ' + _maxHeight + '"></iframe>';

	OpenLightFrame(popupContent);
}



function OpenLightFrame(_popupContent) {
	const lfContainer = document.createElement('div');

	var closePopup = '<div id="closePopup"><a href="javascript:CloseLightFrame();">Close</a></div>'

	lfContainer.id = 'popupContainer';
	lfContainer.innerHTML = _popupContent + closePopup;

	document.body.appendChild(lfContainer);
}



function CloseLightFrame() {
	var lfContainer = document.getElementById('popupContainer');

	if (lfContainer)
	{
		lfContainer.remove();
	}
}