/* To avoid CSS expressions while still supporting IE 7 and IE 6, use this script */
/* The script tag referencing this file must be placed before the ending body tag. */

/* Use conditional comments in order to target IE 7 and older:
	<!--[if lt IE 8]><!-->
	<script src="ie7/ie7.js"></script>
	<!--<![endif]-->
*/

(function() {
	function addIcon(el, entity) {
		var html = el.innerHTML;
		el.innerHTML = '<span style="font-family: \'vta-icon\'">' + entity + '</span>' + html;
	}
	var icons = {
		'vta-vtaplus': '&#xe900;',
		'vta-alert': '&#xe62c;',
		'vta-app-game': '&#xe62d;',
		'vta-back-left': '&#xe62e;',
		'vta-back-right': '&#xe62f;',
		'vta-thumb-down': '&#xe630;',
		'vta-thumb-up': '&#xe631;',
		'vta-check': '&#xe62a;',
		'vta-close': '&#xe62b;',
		'vta-maydoitra': '&#xe619;',
		'vta-new': '&#xe61b;',
		'vta-phone': '&#xe61e;',
		'vta-tablet': '&#xe627;',
		'vta-star': '&#xe600;',
		'vta-360': '&#xe601;',
		'vta-arrow-down': '&#xe602;',
		'vta-arrow-left': '&#xe603;',
		'vta-arrow-right': '&#xe604;',
		'vta-arrow-up': '&#xe605;',
		'vta-award': '&#xe606;',
		'vta-camera': '&#xe607;',
		'vta-cart': '&#xe608;',
		'vta-cart-fast': '&#xe609;',
		'vta-change': '&#xe60a;',
		'vta-comment': '&#xe60b;',
		'vta-compare': '&#xe60c;',
		'vta-date': '&#xe60d;',
		'vta-email': '&#xe60e;',
		'vta-eye': '&#xe60f;',
		'vta-gift': '&#xe610;',
		'vta-hamburge': '&#xe611;',
		'vta-headphone': '&#xe612;',
		'vta-heart': '&#xe613;',
		'vta-house': '&#xe614;',
		'vta-info': '&#xe615;',
		'vta-info-picture': '&#xe616;',
		'vta-laptop': '&#xe617;',
		'vta-location': '&#xe618;',
		'vta-minus': '&#xe61a;',
		'vta-news-widget': '&#xe61c;',
		'vta-pay': '&#xe61d;',
		'vta-pigmoney': '&#xe61f;',
		'vta-playvideo': '&#xe620;',
		'vta-plus': '&#xe621;',
		'vta-product-new': '&#xe622;',
		'vta-sale': '&#xe623;',
		'vta-search': '&#xe624;',
		'vta-sim': '&#xe625;',
		'vta-student': '&#xe626;',
		'vta-telephone': '&#xe628;',
		'vta-truck': '&#xe629;',
		'0': 0
		},
		els = document.getElementsByTagName('*'),
		i, c, el;
	for (i = 0; ; i += 1) {
		el = els[i];
		if(!el) {
			break;
		}
		c = el.className;
		c = c.match(/vta-[^\s'"]+/);
		if (c && icons[c[0]]) {
			addIcon(el, icons[c[0]]);
		}
	}
}());
