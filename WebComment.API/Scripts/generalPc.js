

var homeCredit = [{
    TraTruoc: [{ Value: 20, Name: "20%" }, { Value: 30, Name: "30%" }, { Value: 40, Name: "40%" }, { Value: 50, Name: "50%" }, { Value: 60, Name: "60%" }, { Value: 70, Name: "70%" }],
    ThoiHanVay: [{ Value: 6, Name: "6 tháng" }, { Value: 8, Name: "8 tháng" }, { Value: 9, Name: "9 tháng" }, { Value: 10, Name: "10 tháng" }, { Value: 12, Name: "12 tháng" }, { Value: 15, Name: "15 tháng" }]
}];

var VtaJsPC = VtaJsPC || {} //Namespace


VtaJsPC.service = {
    AddPaymentPC: function (productId, orderType) {
        var ckPaymentName = 'VTAMobilePaymentCK';
        Cookies.set(ckPaymentName, productId + '|' + orderType);

        //if (orderType !== 'TraGop') {
            
        //} else {
        //    Cookies.set('VTAMobileHappyCareCK', "", { expires: -1 });
        //}

    },
    FormatNumber: function (number, decimals, dec_point, thousands_sep) {
        number = (number + '').replace(/[^0-9+\-Ee.]/g, '');
        var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
        // Fix for IE parseFloat(0.55).toFixed(0) = 0;
        s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
        if (s[0].length > 3) {
            s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
        }
        if ((s[1] || '').length > prec) {
            s[1] = s[1] || '';
            s[1] += new Array(prec - s[1].length + 1).join('0');
        }
        // Add this number to the element as text.
        return s.join(dec);
    },
    RenderMoreCategoryProduct: function (ent, callBack) {
        var cateid = $(ent).attr("cateid");
        var page = $(ent).attr("page");
        $.ajax({
            type: 'get',
            data: { "cateid": cateid, "page": page },
            url: '/Category/LoadMoreCate',
            success: function (data) {
                if (typeof (callBack) == "function") {
                    callBack(data);
                }
            }
        });

    },
    GetProductsSearch: function (strSearch) {
        if (strSearch.length < 2 || strSearch.length > 50) {
            $("#searchDialog").html("");
            return;
        }
        $.ajax({
            type: 'get',
            data: { "keyWord": strSearch },
            url: '/Home/SearchHome',
            success: function (data) {
                try {
                    $("#searchDialog").removeClass("hidden");
                    $("#searchDialog").html(data);
                } catch (e) {
                }
            }
        });
    },
    ListCity: function (dropdownId) {
        $.ajax({
            type: 'get',
            url: '/Branch/BranchCity',
            success: function (data) {
                try {
                    $('#' + dropdownId).html("");
                    $('#' + dropdownId).append($('<option />').attr('value', '0').text('===Chọn thành phố==='));
                    $(data).each(function () {
                        var option = $('<option />');
                        option.attr('value', this.Id).text(this.Name);
                        $('#' + dropdownId).append(option);
                    });
                } catch (e) {

                } 
            }
        });
    },
    ListDistrict: function (dropdownId, ent) {
        var districtId = $(ent).val();
        $.ajax({
            type: 'get',
            data: { "id": districtId },
            url: '/Branch/BranchDistrictId',
            success: function (data) {
                try {
                    $('#' + dropdownId).html("");
                    $('#' + dropdownId).append($('<option />').attr('value', '0').text('===Chọn quận huyện==='));
                    $(data).each(function () {
                        var option = $('<option />');
                        option.attr('value', this.Id).text(this.Name);
                        $('#' + dropdownId).append(option);
                    });
                } catch (e) {

                }
            }
        });
    },
    CalcTraGop: function () {
        var dvtg = $('input[name=DonViTraGop]:checked').val();
        var defaultTGTG = $('#ThoiGianTraGop_' + dvtg).val();
        $('#ThoiGianTraGop').val(defaultTGTG);
        var defaultPTTT = $('#PhanTramTraTruoc_' + dvtg).val();
        $('#PhanTramTraTruoc').val(defaultPTTT);
        var specialLx = $('#TraGopType_' + dvtg).val();
        $('#LaiXuatDacBiet').val(specialLx);

        if (specialLx != undefined && specialLx !== "-1") {
            lxTragop = specialLx;
        } else {
            switch (dvtg) {
                case "ACS":
                    lxTragop = 2.2;
                    break;
                case "HDF":
                    lxTragop = 3.05;
                    break;
                case "PPF":
                    lxTragop = 3.22;
                    break;
            }
        }

        var totalPrice = $("#totalPrice").val();
        var tratruocVal = $('#PhanTramTraTruoc').val();
        var thoihanVal = $('#ThoiGianTraGop').val();

        var tratruocPrice = Math.round((totalPrice * tratruocVal / 100) / 1000) * 1000;
        var cl = (totalPrice - tratruocPrice) + (totalPrice - tratruocPrice) * lxTragop * thoihanVal / 100;
        var tThangPrice = Math.round(cl / (thoihanVal * 1000)) * 1000;

        $('p[name=lblTraTruoc').text("0 đ");
        $('p[name=lblTraThang').text("0 đ");
        $('select[name=ddlPhanTramTraTruoc]').attr("disabled", "disabled");
        $('#PhanTramTraTruoc_' + dvtg).removeAttr("disabled");
        $('select[name=ddlThoiGianTraGop]').attr("disabled", "disabled");
        $('#ThoiGianTraGop_' + dvtg).removeAttr("disabled");
        $('select[name=ddlTraGopType]').attr("disabled", "disabled");
        $('#TraGopType_' + dvtg).removeAttr("disabled");
        $('#currentTtruoc_' + dvtg).text(VtaJsPC.service.FormatNumber(tratruocPrice) + " đ");
        $('#currentGThang_' + dvtg).text(VtaJsPC.service.FormatNumber(tThangPrice) + " đ");
        $('#Sotientratruoc').val(tratruocPrice);
        $('#Sotientragop').val(tThangPrice);
    },
    BindDataDropdownSubCategory: function (dropdownId, ent) {
        var parentId = $(ent).val();
        VtaJsPC.service.ListSubCategoryById(parentId, function (data) {
            $('#' + dropdownId).html("");
            $('#' + dropdownId).append($('<option />').attr('value', '0').text('Chọn loại sản phẩm'));
            $(data).each(function () {
                var option = $('<option />');
                option.attr('value', this.CategoryId).text(this.CategoryName);
                $('#' + dropdownId).append(option);
            });
        });
    },
    ListSubCategoryById: function (parentId, callBack) {
        $.ajax({
            type: 'get',
            data: { "parentId": parentId },
            url: '/Category/SubCateByParent',
            success: function (data) {
                if (typeof (callBack) == "function") {
                    callBack(data);
                }
            }
        });
    },
    BindDataDropdownProduct: function (dropdownId, ent) {
        var subCategoryId = $(ent).val();
        VtaJsPC.service.ListProductById(subCategoryId, function (data) {
            $('#' + dropdownId).html("");
            $('#' + dropdownId).append($('<option />').attr('value', '0').text('Chọn sản phẩm'));
            $(data).each(function () {
                var option = $('<option />');
                option.attr('value', this.ProductId).text(this.ProductName);
                $('#' + dropdownId).append(option);
            });
        });
    },
    ListProductById: function (cateroryId, callBack) {
        $.ajax({
            type: 'get',
            data: { "cateroryId": cateroryId },
            url: '/Category/SubProductCate',
            success: function (data) {
                if (typeof (callBack) == "function") {
                    callBack(data);
                }
            }
        });
    },
    GetProductDetailById: function (productId, callBack) {
        $.ajax({
            type: 'get',
            data: { "productId": productId },
            url: '/Payment/GetProductDetailById',
            success: function (data) {
                if (typeof (callBack) == "function") {
                    callBack(data);
                }
            }
        });
    },
    AddName: function (ctr, ent) {
        $('#' + ctr).val($(ent).find('option:selected').text());
    }
}


$(function () {
    
    $('.search-global .input-search').keyup(function (e) {
        VtaJsPC.service.GetProductsSearch($(this).val());
    });

    $('#searchVnpost').keypress(function (e) {
        if (e.which == 13) {
            window.location.href = "tim-kiem?q=" + $('#searchvta').val();
        }
    });

    $('#CategoryPager').on('click', function () {
        $(".spinner").show();
        $('#CategoryPager').hide();
        var page = $('#CategoryPager').attr("page");
        var totalProduct = parseInt($('#lblNumCont').text());
        VtaJsPC.service.RenderMoreCategoryProduct(this, function (data) {
            var $items = $(data);
            $("#cateProductContent").append($items).masonry('appended', $items);
            $(".spinner").hide();
            $('#CategoryPager').show();
            if ((totalProduct - 12) <= 0)
                $('#CategoryPager').hide();
            else {
                $('#CategoryPager').attr("page", parseInt(page) + 1);
                $('#lblNumCont').text(totalProduct - 12);
            }
        });
    });
});


function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}


function InvalidInput(ent) {
    $('#' + ent).addClass("error");
    $("#err-note").show();
    $('.loading-pop-up').hide();
    return false;
};

function strMessage(input) {
    var output = "";
    for (var i = 0; i < input.length; i++) {
        if ((input.charCodeAt(i) === 13) && (input.charCodeAt(i + 1) === 10)) {
            i++;
            output += "<br\>";
        } else {
            output += input.charAt(i);
        }

    }
    return output;
}