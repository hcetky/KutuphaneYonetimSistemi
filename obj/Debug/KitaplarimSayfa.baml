   M S B A M L   `   `   `  ����H  DKKU_Kutuphane, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:KKU_KutuphaneKKU_Kutuphane  5      #    KKU_Kutuphane.KitaplarimSayfa   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -localclr-namespace:KKU_Kutuphane     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Kitaplarım Sayfası��5
       � 	Resources 5      &   System.Windows.Controls.DataGrid �� 
TargetType �� Property �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell��%�  '        '  �    '  �    �� 5   
   "  6   .�5      ��  � ��$ Segoe UI)�6+   �� 5       � ��$ 14(�6)   �� 5       � �� �#3336+   �� 5       � ��
 �����6+   �� 5       �@ RowBackground �#FFF6.   �� 5        �@ AlternatingRowBackground �#F9F9F969   �� 5       � ��$ 1q�60   �� 5       � �� �#DDD6,   �� 5       �@ GridLinesVisibility$ 
Horizontal=�64   �� 5        �@ HorizontalGridLinesBrush �#DDD69   �� 5       �@ VerticalGridLinesBrush
 ���� 67   �� 5       �@ ColumnHeaderHeight$ 40Q�63   �� 5       �@ 	RowHeight$ 35Q�6*   �� 5       �@ CanUserAddRows .  6/   �� 5       �@ CanUserDeleteRows .  62   �� 5       �@ 
IsReadOnly . 6+   �� 5       �@ SelectionMode$ Single=�6.   �� 5        �@ SelectionUnit$ FullRow=�6.   �� 5!       �@ HeadersVisibility$ Column=�62   �� 5"       �@J�SnapsToDevicePixels . 64   �� 5#       � ��$ Stretch=�62   �� 5$       � ��$ Stretch=�64   �� 5%       � ��$ 0,10,0,0q�6'   �� 5&       �@ AutoGenerateColumns .  64   5'      �� 5*   
   "  6   .�5+      ��  � �� �#272B2F6+   �� 5,       � ��
 �����6+   �� 5-       � ��$
 Bold"�6+   �� 5.       � ��$ 14(�6)   �� 5/       � �� �#3336,   �� 50       � ��$ 0 0 1 0q�60   �� 51       � ��$ 5q�6(   52      �� 55   
   "  6   .�56      ��  � �� �#DDD6,   �� 57       � ��$ 0 0 1 0q�60   �� 58       � ��$ 5q�6(   59      5:      .��5<      � $��10q�6    � RowDefinitions 5=   
   �� 5>      $
��Auto �6   �� 5?      $��* �6   .+�5D   
     -   5          Name$ raporDataGrid��5D      $��1��6*   $��16(�67      Columns 5E      0  *System.Windows.Controls.DataGridTextColumn  5F      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Kitap Adı��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 200 6U   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 68   �� *
KitapAdi+6U     5G      $ Teslim Tarihi��6%   $		 200 6]   
 6<   �� *TeslimTarihi+6]     5H      $ Alınan Tarih��6%   $	 * 6[   
 6;   �� *AlinanTarih+6[   5J      5K      5L      