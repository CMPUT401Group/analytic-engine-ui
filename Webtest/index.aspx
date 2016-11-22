<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Webtest.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="datetime-local" name ="metric1" id="mdate1"/>
        <input type="datetime-local" name ="metric2" id="mdate2"/>
         Metric #1:
        <input list="metricname" id="M1" name="mname1"/>           
        <datalist id="metricname">
            <asp:Literal ID="metric1" runat="server"></asp:Literal>
       
            </datalist>
            

            
        Metric #2:
        <input list="metricnames" id="M2" name="mname2"/>
        <datalist id="metricnames">
            <asp:Literal ID="metric2" runat="server"></asp:Literal>
             </datalist>
             

          
        
        <asp:RadioButtonList ID="function" runat="server" >
            <asp:ListItem Selected="True">correlation</asp:ListItem>
            <asp:ListItem >covariance</asp:ListItem>
        </asp:RadioButtonList>
          
        
    </div>
      
         <asp:Button id="Submit"
           Text="Submit"
           OnClick="submit_Click" 
           runat="server" Height="26px"/>

    </form>
</body>
</html>
