<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Webtest.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="datetime-local" name ="metric1"/>
        <input type="datetime-local" name ="metric2"/>
         Metric #1:
        <input list="metricname" id="M1"/>           
        <datalist id="metricname">
            <asp:Literal ID="metric1" runat="server"></asp:Literal>
        </datalist>
        Metric #2:
        <input list="metricnames" id="M2"/>
        <datalist id="metricnames">
           <asp:Literal ID="metric2" runat="server"></asp:Literal>
        </datalist>
    </div>
         <asp:Button id="Submit"
           Text="Submit"
           OnClick="submit_Click" 
           runat="server"/>

    </form>
</body>
</html>
