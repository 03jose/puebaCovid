$("#report").on("click", function () {
    var regionSelected = $("#options:selected").val();
    $.ajax({
        url: $("#refreshSelect").val(),
        type: "post",
        data: {
            regionid: $("#options").val()          
        },
        success: function (results) {
            if (!results.error) {
                $("#options").html('');
                $("#TableInformation").html('');

                var lstoptions = "";
                var rows = (Model.region == 0) ?
                    "<th>REGION </th>" :
                    "<th>PROVINCE </th>";
                rows += "<th>CASES </th><th>DEATHS </th>";
                $(results.lstregions).each(function (index, element) {
                    if (element.RegionId == regionSelected) {
                        lstoptions += "<option selected value='" + element.RegionId + "'>" + element.Name + "</option>"
                    }
                    else {
                        lstoptions += "<option value='" + element.RegionId + "'>" + element.Name + "</option>"
                    }
                });
                $("#options").html(lstoptions);

                $(results.lstTable).each(function (index, element) {
                    rows += "<tr>" +
                                "<td>" + element.ProvName + "</td>" +
                                "<td>" + element.Cases + "</td>" +
                                "<td>" + element.Deaths + "</td>" +
                            "</tr>"
                });

                $("#TableInformation").html(rows);
            }
            else {
                alertify.error(response.message);
            }
        },
        error: function (e1) {
            alertify.error("Error to get province");
        }
    });
})


function fnExport(typeExport) {
    
    $.ajax({
        url: $("#exportDoc").val(),
        type: "post",
        data: {
            typeExport: typeExport,
            region: $("#options").val()
        },
        success: function () {
            
        },
        error: function (e1) {
            alertify.error("Error to export in " + typeExport);
        }
    });

}


$("#btnSaveProvince").on("click", function () {
    
    if ($("#options").length <= 1) {
        alertify.error("Create a new Region first");
        return;
    }

    if ($("#modalRegion2Select:selected").val() == 0)
    {
        alertify.error("Select a Region first");
        return;
    }

    var cases = $("#cases").val()

    var data = {
        Id: $("#modalProvinceSelect:selected").val(),
        Cases:cases ,
        Deaths: $("#death").val(),
        Name: $("#provinceName").val(),
        regionId: $("#modalRegion2Select:selected").val(),
    }

    $.ajax({
        url: $("#SaveProvince").val(),
        type: "post",
        data: {
            information: data
        },
        success: function (results) {
            if (!results.error) {
                alertify.success(results.message);
                if (cases > 0 || document.getElementById("TableInformation").rows.length < 12)
                    $("#report").trigger('click');
            }
            else {
                alertify.error(results.message);
            }
        },
        error: function (e1) {
            alertify.error("Error to save province");
        }
    });
})

$("#btnSaveProvince").on("click", function () {

    var data = {
        RegionId: $("#modalRegionSelect:selected").val(),
        Name: $("#regionName").val(),
    }

    $.ajax({
        url: $("#SaveRegion").val(),
        type: "post",
        data: {
            information: data
        },
        success: function (results) {
            if (!results.error) {
                alertify.success(results.message);
               
            }
            else {
                alertify.error(results.message);
            }
        },
        error: function (e1) {
            alertify.error("Error to save region");
        }
    });
})


$("#modalRegionSelect").on("change", function () {
    var RegionId = $("#modalRegionSelect:selected").val();
    if (RegionId == 0) {
        $("#regionName").val('')
    }
    else {
        $.ajax({
            url: $("#GetRegion").val(),
            type: "post",
            data: {
                RegionId: RegionId
            },
            success: function (results) {
                if (!results.error) {
                    $("#regionName").val(results.Name)
                }
            },
            error: function (e1) {
                alertify.error("Error to get region");
            }
        });
    }
})


$("#modalRegionSelect").on("change", function () {
    var ProvinceId = $("#modalProvinceSelect:selected").val();
    if (ProvinceId == 0) {
        $("#provinceName").val('')
        $("#cases").val(0)
        $("#death").val(0)
        
    }
    else {
        $.ajax({
            url: $("#GetRegion").val(),
            type: "post",
            data: {
                provinceId: ProvinceId
            },
            success: function (results) {
                if (!results.error) {                    
                    $("#provinceName").val(results.province.ProvName)
                    $("#cases").val(results.province.Cases)
                    $("#death").val(results.province.Deaths)
                }
            },
            error: function (e1) {
                alertify.error("Error to get region");
            }
        });
    }
})