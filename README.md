# WebApiDotNetCore calculation for Insurance of Products using SQLite version
#@author A. Eduardo Pimentel

This is a solution to InsuranceAPI calculation for Products using SQLite created in (.NET 3.1)


SQLite Database: Create one Table named "ProductType"


Access to InsuranceController/updateSurcharge endpoint:

Step 1: Run the API and on the top right of the Swagger UI will provide a button "Authorize"
Step 2: Click on the button and add the "access_token" below without the double quotes.
Step 3: If some issues with the token happens or the acess_token expired or copied wrong the API will return Status401Unauthorized. 

 "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjYyeGk1YTlnY08zQlVuUnVDcnN1LSJ9.eyJpc3MiOiJodHRwczovL2Rldi05ODcydWJ1cS51cy5hdXRoMC5jb20vIiwic3ViIjoiak44dWZBV3BBQXplbUh0SWNwZzNXZ21ROFFPSzc3dmxAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzExL3N3YWdnZXIvaW5kZXguaHRtbCIsImlhdCI6MTY0NDU1MzE3MCwiZXhwIjoxNjQ1NTAzNTcwLCJhenAiOiJqTjh1ZkFXcEFBemVtSHRJY3BnM1dnbVE4UU9LNzd2bCIsImd0eSI6ImNsaWVudC1jcmVkZW50aWFscyJ9.Aw6OeOR2YN3u1rj3bBg3JMwEcKwZRfvUUDjghOwtA9ogtgy7I9e7cuqBHjAiBXKZ7ssB37LHPWM6V3sbmF-U_9Z9uBUlQdPnu5r4fi3pNmzuDZmtdLG9oZzSpFiqweB85sHtjDP1JGJq3CADMPgAultZQcu28xBtpaVm7Jc2hEng5Wjx6BT8SIE0gBBwNbTZBPsrbf3RNH6smhikHtWhG2-KtjGOCX-VeuAJjhbngc0t0-UqfkrcfbxhQSn2EW9ezF3XWpzK-fHyS5GoxYFNPFkxezGDIwKJzrGofeynp8ozNuBFjurroXITGoEO8iwN3tjLVK-Z2ez5qRIl1_3-VA"


INFO: For a development version propose the access_token is also provided in the appsettings.json
