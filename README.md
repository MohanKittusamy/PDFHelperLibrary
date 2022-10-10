# PDFHelperLibrary

This is a simple .NET 6 Library to help in create & read pdf document. This is not dependent on any other third party dll or application, hence you can directly use this solution to create/read the pdf documents.

To prepare a solution that can be extended in the future based on the requirements and new features, we are making this solution as extendible so that it supports future changes with minimal change in code.

To test easily, we have included a console app that can be used to test out the complete functionality without any issues.

In addition to the reading / writing functionality in PDF document, we have the following operations as listed below:

· Font

· FontSize

· DocumentType

· PageOrientation

· ContentAlignment

· ContentType

Font - This is to set which font styling type the text content in the pdf document will be used while generating the document

FontSize - This helps to setup the size of the content to be used for each tect content in the pdf document

DocumentType - This helps to define the document type / size like A4, Letter etc., on which the pdf output layout will be selected

PageOrientation - This section helps us to define the page orientation whether the page is in landscape or portrait orientation. We can declare true for LandScape and False for Portrait orientation

ContentAlignment - This is used to define the alignment of the content to be used in the pdf document either a text content, image, table etc., The location of the content on where it has to be used in the PDF document like Left, Right, Center, Justify etc.,

ContentType - This is used to define the content type like the text, image, video, table etc., This will help us to extend the functionality of the PDF helper tp handle any type of content type
