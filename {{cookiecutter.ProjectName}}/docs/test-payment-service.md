
## Step 1 : Testing Banking and Fraud Service
- **Test for existing account using Bank Service** : Do a **GET** to http://localhost:9001/checkDetails?accountNumber=12345&ifscCode=AXIS1234, we should see the response `200 OK`

- **Test for non-existing account using Bank Service** : Do a **GET** to http://localhost:9001/checkDetails?accountNumber=123&ifscCode=AXIS1234, we should see the response `404 Not Found`

```
{
    "timestamp": "2022-02-07T05:05:25.526+00:00",
    "status": 404,
    "error": "Not Found",
    "path": "/checkDetails"
}
```

- **Test same beneficiary and payee account numbers using Fraud Service** : Do a **POST** to http://localhost:9002/checkFraud with below payload having same account number for both beneficiary and payee.
```
  {
        "amount": 10001,
        "beneficiaryName": "May",
        "beneficiaryAccountNumber": 67890,
        "beneficiaryIfscCode": "HDFC1234",
        "payeeName": "Ethan",
        "payeeAccountNumber": 67890,
        "payeeIfscCode": "HDFC1234",
        "status": "Success"
    }
```
We should get `422 Unprocessable Entity` response as shown below.
```
{
    "timestamp": "2022-02-07T05:06:03.325+00:00",
    "status": 422,
    "error": "Unprocessable Entity",
    "path": "/checkFraud"
}
```

## Step 2 : Interacting with {{cookiecutter.ProjectName}} Service
- Do a **GET** to http://localhost:5000/health, we should see the response `Healthy`.

### Seeding the Bank Information data which is a prerequisite for {{cookiecutter.ProjectName}} Service
- Do a **POST** to http://localhost:5000/api/bank
    - With JSON body 
```
{ 
   "ifscCode": "HDFC1234",
   "name": "HDFC" 
}
```
 - We should get a 201 response
        - With following body `{ "bankId": 6 }`

- Do a **GET** to http://localhost:5000/api/bank/1
    - We should get a 200 response
        - With following JSON 
         `{
          "id": 2,
          "ifscCode": "HDFC1234",
          "name": "HDFC"
          }`


### Testing the {{cookiecutter.ProjectName}} Service
- Do a **POST** to http://localhost:5000/api/{{cookiecutter.ProjectName}}
    - With JSON body `
  {
      "amount": 10001,
      "payee": {
      "name": "Ethan",
      "accountNumber": 12345,
      "ifscCode": "HDFC1234"
      },
      "beneficiary": {
      "name": "May",
      "accountNumber": 67890,
      "ifscCode": "HDFC1234"
      }
  }`
    - We should get a 201 response
        - With following body `{
          "{{cookiecutter.ProjectName}}Id": 2
          }`

- Do a **GET** to http://localhost:5000/api/{{cookiecutter.ProjectName}}/1
    - We should get a 200 response
        - With following JSON :

```
{
    "id": 1,
    "amount": 10,
    "beneficiary": {
        "name": "user1",
        "accountNumber": 12345,
        "ifscCode": "HDFC1234"
    },
    "payee": {
        "name": "user3",
        "accountNumber": 67890,
        "ifscCode": "AXIS1234"
    }
}
```


