<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/Admin.Master" CodeBehind="ImportExport.aspx.vb" Inherits="Fiscus.ImportExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    

       <nav class="page-breadcrumb">
					<ol class="breadcrumb">
						<li class="breadcrumb-item"><a href="#">Admin</a></li>
						<li class="breadcrumb-item active" >Member Import & Export</li>
					</ol>
				</nav>

          <div class="card card-body ">
			  <div class="card-title">
				  Import Members
			  </div>
			          <div class="form-group">
            <div class="col-md-6">
                <label class="col-form-label">File to Upload</label>
            </div>
            <div class="col-md-6">

                <div class="form-group row">    
                    <label class="col-form-label">Select Branch</label>
                    <div class="col-md-6">
                        <select id="branch" class="custom-select">
                            <option value="01">Karavilai</option>
                            <option value="02">Villukuri</option>
                            <option value="03">Mekkamandapam</option>
                            <option value="04">Alencode</option>
                            <option value="05">Padmanapapuram</option>
                        </select>
                    </div>
                </div>
                
            </div>
        </div>
        <div class="form-group">
            <input type="button" value="Upload" name="btnUpload" onclick="uploadfile();" class="btn btn-sm btn-primary rounded-0 text-uppercase" />
        </div>

		  </div>

    <div class="card card-body ">
			  <div class="card-title">
				  Export Members
			  </div>
			 
        <div class="form-group">

            <a href="http://192.168.2.12:8080/api/member/export"  target="_blank" class="btn btn-sm btn-primary rounded-0 text-uppercase" >Export</a>
        </div>

		  </div>
    <link rel="stylesheet" href="../css/HoldOn.min.css" />
    <script src="../js/HoldOn.min.js" type="text/javascript" ></script>
    

        <script type="text/javascript">
        //const { post, error } = require("jquery");

        // Add the following code if you want the name of the file appear on select
        $(".custom-file-input").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });


            function ExportFile() {


                HoldOn.open({
                    theme: 'sk-fading-circle',
                    message: "<br\><h4> Please Wait....</h4>"
                });

                var url = "http://192.168.2.12:8080/api/member/export";


                fetch(url, {
                    method: 'get', mode: 'cors'
                })
                headers.append('Access-Control-Allow-Origin', '*')

                    .then(response => {
                        //alert(response.json);

                        //  modalLoading.init(true);
                        HoldOn.close();
                        alert("File Exported successfully to E:\Backup");

                    })
                    .catch(error => {
                        console.log(error)
                        error.json().then((body) => {
                            //Here is already the payload from API
                            alert(body);
                            console.log(body);
                        });
                        HoldOn.close();
                        alert("Error occurs");
                    });
            
                
       /*         jQuery.ajax({
                    type: 'GET',
                    url: url,
                    crossDomain: true,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (repo) {
                        if (repo.status == "Ok") {
                            //  modalLoading.init(true);
                            HoldOn.close();
                            alert("File Exported successfully to E:\Backup");


                        }
                    },
                    error: function (xhr, status) {
                        alert(xhr.status);
                        HoldOn.close();
                    }

                });
                         */
                
            }


            function uploadfile() {

                

                var br = $('#branch').val();
                var url = "http://192.168.2.12:8080/api/member/import/" + br;

                window.open(url, '_blank');

          /*     //  modalLoading.init(false);

                HoldOn.open({
                    theme: 'sk-fading-circle',
                    message: "<br\><h4> Please Wait....</h4>"
                });

            var company = $('#company').val();
            var files = $('#customFile').prop("files");
               
            formData = new FormData();
                formData.append("formFile", files[0]);
               fetch(url, { method: 'POST', mode: 'cors', body: formData })
                    .then(response => function () {
                        HoldOn.close();
                        alert("File : " + repo.filename + " is uploaded successfully");
                    })
                    .catch(error => {

                        alert(error);
                    })
            
            jQuery.ajax({
                type: 'POST',
                url: 'http://192.168.2.12:8080/api/member/import',
                data: formData,
                crossDomain: true,
                headers: {
                    'Access-Control-Allow-Origin': '*'
                },
                cache: false,
                contentType: false,
                processData: false,
                success: function (repo) {
                    alert(repo.status);
                                   if (repo.status == "success") {
                        //  modalLoading.init(true);
                        HoldOn.close();
                        alert("File : " + repo.filename + " is uploaded successfully");
                        

                    }
                },
                error: function (err) {
                    alert(err.status)
                    HoldOn.close();
                    alert("Error occurs");

                }

            });
            */

        }


        
        </script>

</asp:Content>
