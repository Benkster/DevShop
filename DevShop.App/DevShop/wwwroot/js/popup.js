function LightFrame(_url, _width, _height, _reloadPage) {
	_reloadPage = (_reloadPage) ? _reloadPage : false;

	var popupSettings = 'src="' + _url + '" width="' + _width + '" height="' + _height + '"';
	var containerSettings = 'width: ' + _width + '; height: ' + _height + ';'	

	OpenLightFrame(popupSettings, containerSettings, _reloadPage);
}



function LightFrame(_url, _width, _height, _maxWidth, _maxHeight, _reloadPage) {
	_reloadPage = (_reloadPage) ? _reloadPage : false;

	var popupSettings = 'src="' + _url + '" width="' + _width + '" height="' + _height + '" style="max-width: ' + _maxWidth + '; max-height: ' + _maxHeight + '"';
	var containerSettings = 'width: ' + _width + '; height: ' + _height + '; max-width: ' + _maxWidth + '; max-height: ' + _maxHeight + ';';

	OpenLightFrame(popupSettings, containerSettings, _reloadPage);
}



var allowClose = false;

function OpenLightFrame(_popupSettings, _containerSettings, _reloadPage) {
	allowClose = false;

	const lfContainer = document.createElement('div');
	var popupContent = '<iframe id="popupIframe" class="noLine" ' + _popupSettings + '></iframe>';

	var closePopup = '<div id="closePopup"><a href="javascript:CloseLightFrame(' + _reloadPage + ');"></a></div>'

	lfContainer.id = 'popupContainer';
	lfContainer.innerHTML = '<div id="popupIframeContainer" class="fixed centerPos backClearColor thickDarkGrayLine" style="' + _containerSettings + '">' + popupContent + closePopup + '</div>';
	lfContainer.addEventListener('click', CloseLightFrame);

	document.body.appendChild(lfContainer);

	allowClose = true;
}



function CloseLightFrame(_reloadPage) {
	if (allowClose)
	{
		_reloadPage = (_reloadPage) ? _reloadPage : true;


		var lfContainer = document.getElementById('popupContainer');

		if (lfContainer)
		{
			lfContainer.remove();
		}



		if (_reloadPage) 
		{
			location.reload();
		}
	}
}