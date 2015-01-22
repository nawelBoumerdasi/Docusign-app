<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactInformation.aspx.cs" Inherits="DocuSignApp.ContactInformation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        Name: <input id="Name" name="Name" type="text" /><br />
        Email: <input id="Email" name="Email" type="text" /><br />
        Use a local document: <input id="RadioLocal" name="Radio1" type="radio" value="RadioLocal" />
        Use a template: <input id="RadioTemplate" name="Radio1" type="radio" value="RadioTemplate" /> <br />
        <input id="Submit" type="submit" value="submit" />
    </form>
    <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
</body>
</html>
