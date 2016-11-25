﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Webtest.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="metroui\css\metro.css" rel="stylesheet" />
    <link href="metroui\css\metro-icons.css" rel="stylesheet" />
    <script src="metroui\js\jquery.js"></script>
    <script src="metroui\js\metro.min.js"></script>
    <title>Analytics Engine - Covariances and Correlation</title>
</head>
<body class="bg-orange fg-white">
    <div class="grid">

        <div class="container padding20">
            <div class="row">
                <h1>Analytics Engine - Covariances and Correlation</h1>
               
            </div>
            <div class="row">
                 <form id="form1" runat="server">
                    <div class="input-control text">
                        <label>From:</label>
                        <input type="datetime-local" name="metric1" id="mdate1" />
                         
                    </div>
                     <div class="input-control text">
                         <label>To:</label>
                         <input type="datetime-local" name="metric2" id="mdate2" />
                     </div>

                    <div  class="input-control text">
                       
                        <label>Metric #1:</label>
                    <input list="metricname" id="M1" name="mname1" />
                        <datalist id="metricname">
                            <asp:Literal ID="metric1" runat="server"></asp:Literal>

                        </datalist>
                    </div>
                    <div  class="input-control text">
                        <label>Metric #2:</label>
                        <input list="metricnames" id="M2" name="mname2" />


                        <datalist id="metricnames">
                            
                            <asp:Literal ID="metric2" runat="server"></asp:Literal>
                            </datalist>
                    </div>
                    <div>
                        <asp:RadioButtonList ID="function" runat="server" >
                            <asp:ListItem Selected="True">correlation</asp:ListItem>
                            <asp:ListItem>covariance</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <asp:Button ID="Submit"
                        Text="Submit"
                        OnClick="submit_Click"
                        runat="server" Height="26px" />
                    
                     <div>
                        <asp:Label runat="server" ID="result_display" Visible="False"></asp:Label>
                     </div>
                    
                </form>
            </div>



        </div>
    </div>









</body>
</html>
