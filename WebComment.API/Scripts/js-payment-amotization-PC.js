function ddlNgheNghiepChange(ent) {
    if ($(ent).find('option:selected').val() === "1") {
        $("#nghenghiep_option").slideDown();
    } else {
        $("#nghenghiep_option").slideUp();
    }
}

function SelectStoreRecieve(ent) {
    if ($("#ProductId").val() === "0") {
        alert("Bạn vui lòng chọn sản phẩm trả góp!!!");
        return false;
    }

    $('.loading-pop-up').show();
    var storeCode = $(ent).find('option:selected').attr("storecode");
    $('#BranchsName').val($(ent).find('option:selected').text());
    $('#StoreCode').val(storeCode);
    var isSinhVien = $('#IsStudent').attr("checked") === "checked";
    BindDataTraGop($("#ProductId").val(), storeCode, isSinhVien);
}

function CheckValidSubmit() {
    debugger;
    if ($("#ProductId").val() === "0") {
        alert("Bạn vui lòng chọn sản phẩm trả góp!!!");
        return false;
    }
    $('.loading-pop-up').show();
    var Fullname = $("#Fullname").val();
    var Email = $("#Email").val();
    var Phone = $("#Phone").val();
    var Birthday = $("#Birthday").val();
    var Cmnd = $("#Cmnd").val();
    var Address = $("#Address").val();
    var GiayToCanCo = $('input[name=GiayToCanCo]:checked').val();
    var CityHoKhauId = $("#CityHoKhauId").val();
    var CityNoiOId = $("#CityNoiOId").val();
    var NgheNghiepName = $("#NgheNghiepName").val();
    var CityId = $("#CityId").val();
    var DistrictId = $("#DistrictId").val();
    var Color = $("input[name=Color]:checked").val();
    $("#ProductColorCode").val(Color);
    dob = new Date(Birthday.replace(/(\d{2})[-/](\d{2})[-/](\d{4})/, "$2/$1/$3"));
    var today = new Date();
    var age = Math.floor((today - dob) / (365.25 * 24 * 60 * 60 * 1000));
    if (age < 18) {
        alert("Bạn chưa đủ tuổi để có thể tham gia chương trình trả góp. Vui lòng liên hệ Viễn thông A để biết thêm chi tiết");
        return InvalidInput("Birthday");
    }
    $("#Old").val(age);
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var phoneReg = /^[0-9]{6,13}$/;

    if (Fullname == "") {
        return InvalidInput("Fullname");
    }
    if (Email === "") {
        return InvalidInput("Email");
    }
    if (!emailReg.test(Email)) {
        return InvalidInput("Email");
    }
    if (Phone == "0") {
        return InvalidInput("Phone");
    }
    if (!phoneReg.test(Phone)) {
        return InvalidInput("Phone");
    }
    if (Birthday === "") {
        return InvalidInput("Birthday");
    }
    if (Cmnd === "") {
        return InvalidInput("Cmnd");
    }
    if (Address === "") {
        return InvalidInput("Address");
    }
    if (GiayToCanCo === "") {
        return InvalidInput("GiayToCanCo");
    }
    if (CityHoKhauId === "0") {
        return InvalidInput("CityHoKhauId");
    }
    if (CityNoiOId === "0") {
        return InvalidInput("CityNoiOId");
    }
    if (NgheNghiepName === "0") {
        return InvalidInput("NgheNghiepName");
    } else if (NgheNghiepName === 1 && $("#NgheNghiepNameOption") === "") {
        return InvalidInput("NgheNghiepName");
    }

    if (CityId === "0") {
        return InvalidInput("CityId");
    }
    if (DistrictId === "0") {
        return InvalidInput("DistrictId");
    }
    if (Color == undefined) {
        $(".color-error").show();
        return InvalidInput('Color');
    }

    return true;
}

function InvalidInput(ent) {
    $('#' + ent).addClass("error");
    $("#err-note").show();
    $('.loading-pop-up').hide();
    return false;
}

function BindDataTraGop(productId) {

    VtaJsPC.service.GetProductDetailById(productId, function (data) {
        $("#ProductId").val(data.Product.ProductId);
        $("#ProductCode").val(data.Product.ProductCode);
        $("#totalPrice").val(data.Product.PriceHeadOffice);
        $("#pnlProductTragop #productPrice").text(VtaJsPC.service.FormatNumber(data.Product.PriceHeadOffice) + "đ");
        $("#pnlProductTragop #productName").text(data.Product.ProductName);
        var htmlPromotionProduct = data.Product.htmlPromotionProduct;
        $("#pnlProductTragop #pnlPromotion").html(htmlPromotionProduct);
        $("#pnlProductTragop #imgtragop").attr("src", data.Product.ImagePart);

        var colorList = '<p>Chọn màu</p><ul class="choose-color list-inline">';
        $(data.ListColor).each(function () {
            colorList = colorList + '<li><div class="radio"><label><input checked="checked" id="Color" name="Color" type="radio" value="' + this.ProductColorCode + '"><div class="color" style="background-color: #' + this.ColorCode + '" data-colorcode="' + this.ProductColorCode + '"></div ></label></div></li>';
        });
        colorList = colorList + '</ul>';

        $("#listColorId").html(colorList);

        $("#pnlProductTragop").show();

        VtaJsPC.service.CalcTraGop();
        $('.loading-pop-up').hide();

    });
}

function LoadDataTraGop() {
    
    var lstDatahomeCredit = homeCredit;
    $('#PhanTramTraTruoc_PPF').html("");
    $(lstDatahomeCredit[0].TraTruoc).each(function () {
        var option = $('<option />');
        option.attr('value', this.Value).text(this.Name);
        $('#PhanTramTraTruoc_PPF').append(option);
    });

    $('#ThoiGianTraGop_PPF').html("");
    $(lstDatahomeCredit[0].ThoiHanVay).each(function () {
        var option = $('<option />');
        option.attr('value', this.Value).text(this.Name);
        $('#ThoiGianTraGop_PPF').append(option);
    });
}

function EnablePanelTraGop(priceCurrent) {
    var isCombo = $('#IsCombo').val();
    if (isCombo !== "true") {
        $("#pnl_tragop_PPF").hide();
        $("#pnl_tragop_HDF").hide();
        $("#pnl_tragop_ACS").hide();
        if (priceCurrent >= 1500000) {
            $("#pnl_tragop_PPF").show();
        }
        if (priceCurrent >= 2500000) {
            $("#pnl_tragop_HDF").show();
        }
        if (priceCurrent >= 3300000) {
            $("#pnl_tragop_ACS").show();
        }
    }
}

$(function () {
    $('#dllSelectedMainCategory').change(function () {
        VtaJsPC.service.BindDataDropdownSubCategory("dllSelectedSubCategory", this);
    });
    $('#dllSelectedSubCategory').change(function () {
        VtaJsPC.service.BindDataDropdownProduct("dllSelectedProduct", this);
    });

    $('select[name=ddlTraGopType]').change(function () {
        VtaJsPC.service.CalcTraGop();
    });

    $('#dllSelectedProduct').change(function () {
        $('.loading-pop-up').show();
        BindDataTraGop(this.value);

    });

    $("input[name=DonViTraGop]:radio").change(function () {

        $('div[name=pnlDVTG').removeClass("checked");
        $('#pnlDVTG_' + this.value).addClass("checked");
        VtaJsPC.service.CalcTraGop();
    });

    $("select[name=ddlPhanTramTraTruoc]").change(function () {
        VtaJsPC.service.CalcTraGop();
    });

    $("select[name=ddlThoiGianTraGop]").change(function () {
        VtaJsPC.service.CalcTraGop();
    });
    LoadDataTraGop();
    VtaJsPC.service.CalcTraGop();
})