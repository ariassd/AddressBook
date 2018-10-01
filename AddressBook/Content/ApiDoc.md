API
=======

API RestFul for Contacts

This is an API REST for contacts, you could use this for retrieve and send new data to the system

The base url is the following:

.../api/Contact


```js
http://{SYSTEM URL}/api/Contact/

```


##How to use

The API is working with the next http verbs: 
```html
http://{SYSTEM URL}/api/Contact/

```

###Http Get:
Getting information into the system

###Http Post:
Insert information to the system

###Http Put:
Update information in the system

###Http Delete:
Delete information from the system

The information is only available in **.json** format

All responses have three main nodes Content, Code and Message, The node Content contains the result of the request and the nodes Code and Message are simple information about the process
This is an example of a simple response. 
```js
{
    "Content": [
        {
            "Id": "5bae98761a04400bc6477d20",
            "Name": "Luis Diego",
            "LastName": "Arias",
            "Phone": "2345-1233",
            "Email": "ariassd@mail.com",
            "Notes": "Simple contact",
            "Status": "ac"
        }
    ],
    "Code": 200,
    "Message": ""
}

```


##Models

### Contact

It contains the main information about an contact

| id  | Name  | LastName  | Phone | Email | Notes | Status |
|-----|-------|-----------|-------|-------|-------|--------|
| UI  |String | String    |String | String|String | String |


## Available requests for the API


### Get all list of contacts

The following request return the whole list of contacts

**HTTP VERB GET**

http://{SYSTEM URL}/api/Contact/

```html
http://{SYSTEM URL}/api/Contact/
```

```js
RESPONSE
{
    "Content": [
        {
            "Id": "5bae98761a04400bc6477d20",
            "Name": "Luis Diego",
            "LastName": "Arias",
            "Phone": "2345-1233",
            "Email": "ariassd@mail.com",
            "Notes": "Simple contact",
            "Status": "ac"
        }
    ],
    "Code": 200,
    "Message": ""
}
```

### Get contact by ID

This is the way to get a Contact using the ID of the system

**HTTP VERB GET**

http://{SYSTEM URL}/api/Contact/{id}

```html
http://{SYSTEM URL}/api/Contact/5bae98761a04400bc6477d20
```

```js
RESPONSE
{
    "Content": [
        {
            "Id": "5bae98761a04400bc6477d20",
            "Name": "Luis Diego",
            "LastName": "Arias",
            "Phone": "2345-1233",
            "Email": "ariassd@mail.com",
            "Notes": "Simple contact",
            "Status": "ac"
        }
    ],
    "Code": 200,
    "Message": ""
}
```

### Get contact by Name

This is the way to get the list of contact looking for the name of the contact

**HTTP VERB GET**

http://{SYSTEM URL}/api/Contact/name/{name}

```html
http://127.0.0.1:8080/api/Contact/name/luis
```

```js
RESPONSE
{
    "Content": [
        {
            "Id": "5bae98761a04400bc6477d20",
            "Name": "Luis Diego",
            "LastName": "Arias",
            "Phone": "2345-1233",
            "Email": "ariassd@mail.com",
            "Notes": "Simple contact",
            "Status": "ac"
        },
        {
            "Id": "5bafd4b51a04400e4786a50a",
            "Name": "Luis Fernando",
            "LastName": "Quesada",
            "Phone": "060987654",
            "Email": "fquesada@mail.com",
            "Notes": "Pour man",
            "Status": "ac"
        }
    ],
    "Code": 200,
    "Message": ""
}
```

### Create a new contact

This is the way to create a new contact in the system

**HTTP VERB POST**

http://{SYSTEM URL}/api/Contact/

```html
http://127.0.0.1:8080/api/Contact
```

```js
REQUEST

{
    "Id": "",
    "Name": "First name",
    "LastName": "Last Name",
    "Phone": "0987-1234",
    "Email": "contact@mail.com",
    "Notes": "Simple contact",
    "Status": "ac"
}

```

```js
RESPONSE
{
    "Code": 201,
    "Message": "New contact created"
}
```



### Update an existing contact

This is the way to create a new contact in the system

**HTTP VERB PUT**

http://{SYSTEM URL}/api/Contact/{id}

```html
http://127.0.0.1:8080/api/Contact/5bb14de81a0440023efa2890
```

```js
REQUEST

{
    "Id": "",
    "Name": "First name",
    "LastName": "Last Name",
    "Phone": "0987-1234",
    "Email": "contact@mail.com",
    "Notes": "Simple contact",
    "Status": "ac"
}

```

```js
RESPONSE
{
    "Code": 201,
    "Message": "Modified"
}
```
### Delete an existing contact

This is the way to detete an existing contact in the system

**HTTP VERB DELETE**

http://{SYSTEM URL}/api/Contact/{id}

```html
http://127.0.0.1:8080/api/Contact/5bb14de81a0440023efa2890
```

```js
REQUEST

{
    "Id": "",
    "Name": "First name",
    "LastName": "Last Name",
    "Phone": "0987-1234",
    "Email": "contact@mail.com",
    "Notes": "Simple contact",
    "Status": "ac"
}

```

```js
RESPONSE
{
    "Code": 200,
    "Message": "Contact has deleted"
}
```

##About
2018 - Api RestFul for address book
Author Luis Arias
