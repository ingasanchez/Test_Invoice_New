# Test_Invoice_New

Práctica realizada como requerimiento para puesto de trabajo en el area de programación, para la empresa Schad.

- Esta prueba fue realizada en .Net, utilizando MVC y EntityFramework como ORM. 

Notas de lugar: 
Fue necesario cambiar la clave foránea utilizada en la tabla InvoiceDetail, la cual se estaba vinculando mediante el atributo CustomerID con la tabla Invoice, lo que hace imposible clasificar los detalles de cada factura generada debido a que la clave foranea era el atributo CustomerID, el cual esta presente tanto en la tabla Invoice como en la tabla InvoiceDetail. 

Debido a esta problematica, reemplacé el atributo InvoiceDetail.CustomerID por el atributo InvoiceDetail.InvoiceId, el cual hace referencia foránea con el atributo Invoice.Id, lo que permite que se puedan clasificar los detalles con sus respectivos header, el cual tiene contenido al cliente, quien es la entidad a la que se le realiza la factura y que en lo adelante no se estará repitiendo por cada detalle, cumpliendo asi reglas de normalización.  

El uso de Delegate que pude aplicar fue para calcular dinámicamente los totales a ser incorporados en la tabla Invoice, los cuales son mostrados y actualizados temporalmente cada vez que se agrega un InvoiceDetail.

Cualquier duda, pregunta y o sugerencia quedo a su entera disposición.

Att, Ing. Adison Sánchez.
