# Delivery Service

��� ���������� ������������� ��� ���������� ������� �� ������ � ������� ��������. 
��� ���������� �������� ������ ����������� � ������������ �� �����.


## ���������

1. � DeliveryService.ConsoleApp/Data � ����� orders.csv ������� ������ ��� ���������� 
(���� ������ ���� � ������� .csv)

2. � ����� ������������ appsettings.json � DeliveryService.ConsoleApp ����������� ���� 
��� ����������� ���� ��� ��������� ����� ������ ��������, ��� ��������� ��������������� ������ � ��� ��������� ���� �������� 
�� ������� ���� ����� ��������.

## ������ 

����� ��������� ����������, ��������� ��������� ������� �� ����� DeliveryService.ConsoleApp:

dotnet run <district> <firstDeliveryTime>

���:
   <district> � ����� �������� (��������, Downtown).
   <firstDeliveryTime> � ����� ������ �������� � ������� ����-��-�� ��:��:�� (��������, 2024-10-24 12:00:00).

### ������ �������

dotnet run Downtown "2024-10-24 12:00:00"

### ���������

����� ���������� ���������:
   1. ��������������� ������ ����� ��������� � ���� Data/filtered_orders.csv.
   2. ���� ����� �������� � ���� logs/delivery_log.txt.




