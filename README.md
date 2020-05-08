# README

:wave: Hello! This is an experimental extension on integrating Visual Studio with different source code
search engines. Currently it only supports Sourcegraph, but it could also easily integrate with others
(e.g. OpenGrok).

If you choose to give it a try and install it, first set your Sourcegraph URL and access token on
**Tools > Options > Code Search**. Then, right click on any symbol in the code editor and you will see
a new menu option named "Find All References with Code Search"--click it and it will trigger the search.

Please be aware of two points:
1. It currently ignores whether the SSL certificate of Sourcegraph is valid or not. This is for testing
   purposes, so I'll make this an option in the future.
2. Your access token is stored with the rest of your options, which are not encrypted.