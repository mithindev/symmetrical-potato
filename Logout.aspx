<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logout.aspx.vb" Inherits="Fiscus.Logout" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Good Bye...</title>
    <link href="../css/core.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />

     <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
     </script>
    
  
</head>

<body onload="preventBack();" >



    <div class="main-wrapper" >
        <div class="page-wrapper full-page"  >
            <div class="page-content d-flex align-items-center justify-content-center" style="background-image: url('../Images/bg-3.jpg')">

                <div class="row w-100 mx-0 auth-page">
                    <div class="col-md-8 col-xl-6 mx-auto" ">
                        <div  >
                            <div class="row">
                                
                                <div class="col-md-12 pl-md-0">
                                    <div class="auth-form-wrapper px-4 py-5">
                                        <a href="#" class="noble-ui-logo  d-block mb-2 text-center "><span>Fiscus beta</span></a>
                                        <h5 class="text-muted font-weight-normal mb-4 text-center ">GOOD BYE ! For Now</h5>
                                        
							
                                        <div class="dropdown-header  d-flex flex-column align-items-center">
                                            <div class="figure mb-3">

                                                <img src="..\images\user.png" height="96" width="96" />
                                            </div>
                                            <div class="info text-center">
                                                <p class="name font-weight-bold mb-0" style="color:#fff !important">Hi !&nbsp;<% =Session("sesusr")%></p>
                                                <p class="email text-muted mb-3">you have Successfully Logged Out.</p>
                                            </div>
                                            <a href="Login.aspx" class="btn btn-outline-primary ">Log In</a>
                                        </div>
                                            
                                            </div>
                                        </div>
                                    </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    
</body>
</html>


						

