<system.net>
    <mailSettings>
        <smtp deliveryMethod="Network" from="SomeWebsite Admin &lt;someuser@gmail.com&gt;">
            <network host="smtp.gmail.com" port="587" enableSsl="true" defaultCredentials="false" 
			userName="someuser@gmail.com" password="somePassword" />
        </smtp>
    </mailSettings>
</system.net>


Mailer.Instance.DisableCertificateCheck();
Mailer.Instance.SendMain("subject","body", "sample@address.com");
