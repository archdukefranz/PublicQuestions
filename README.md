
Installation
-----------

Create a PublicQuestions_AddIns directory at the same level as the Public Questions.

Download the following addtional tools needed to develop the site.
NUnit(http://www.nunit.org/?p=download) - UnitTest infrastructure
RavenDB(http://ravendb.net/download/) - The batabase engine for the application

Place these files into the PublicQuestions_AddIns directory

Usage 
-----
Start the database by running Raven_Startup.bat.  This should start the database server on port 8080.
All data is stored in the ravenDB\Server\Data folder.

Testing
-------
From the command prompt type the following, changing the <path-to-code> to the location on your PC:
runas /user:administrator "<path-to-code>\PublicQuestions\UnitTests_startup.bat"

Contributing
------------
Contribution are welcome.
