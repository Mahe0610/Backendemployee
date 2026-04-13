# Certificate Notes

- PDF is generated in A4 format on backend through QuestPDF.
- Company logo should be placed at backend path: `Assets/logo.png`.
- Barcode is generated with CODE_128 and includes certificate document code.
- Excel/PDF export endpoints are:
  - `GET /api/certificate/{employeeId}/excel`
  - `GET /api/certificate/{employeeId}/pdf`
