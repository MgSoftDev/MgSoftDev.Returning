# MgSoftDev.Returning

This is a library, which serves to improve the return of a function.
We can not only return a value, but also return an error, handle exceptions, save error logs.

# Quick start

Add the following nugget package
**MgSoftDev.Returning**


## Return Void Method 
 The Returning class is used to represent a void method.
This class can be returned representing an error or as a successful execution.

    Returning SendMail(string to, string message)  
    {
	    try  
	    {  
		    //... code to send Message  
		    return  Returning.Success();  
	    }  
	    catch (Exception ex)  
	    {
		    return  Returning.Error("Error to send mail",ex);  
	    }  
      
    }
