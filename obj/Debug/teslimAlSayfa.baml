   M S B A M L   `   `   `  ����H  DKKU_Kutuphane, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:KKU_KutuphaneKKU_Kutuphane  5      !    KKU_Kutuphane.teslimAlSayfa   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:KKU_Kutuphane     5        N� Title$  Teslim Alma Sayfası��5	       N� 
Background �	#FFF0F0F0 N� 
FontFamily$ Segoe UI)�5       � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell��%�  '��       '  &    '  0    '  �    '��     �� 5   
   " ��6   .�5      ��  � ��$ Segoe UI)�6+   �� 5       � ��$ 14(�6)   �� 5       � ��
 �����6+   �� 5       � �� �#5C7AEA6+   �� 5       � ��$ 36��6'   �� 5       � ��$	 100��6&   �� 5       � ��$ 10q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ 0q�60   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       �@J�SnapsToDevicePixels . 64   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 4��6!   #��z��62   ._�5       �� $��Center=�6/   $��Center=�6L   5!      5"      5$      	 5%      P� 5&      
 �@J�IsMouseOver . . �5'      ��  � �� �#4A69BD63   5(      5*      �� 5-   
   "  6   .�5.      ��  � ��
 �����6+   �� 5/       � ��$ 14(�6)   �� 50       � �� �#3336+   �� 51       �@ 	RowHeight$ 35Q�6*   �� 52       �@ HeadersVisibility$ Column=�62   �� 53       �@ GridLinesVisibility$ 
Horizontal=�64   �� 54        �@ HorizontalGridLinesBrush �#DDD69   �� 55       � �� �#DDD6,   �� 56       � ��$ 1q�60   �� 57        �@ AlternatingRowBackground �#F9F9F969   58      �� 5;   
   "  6   .�5<      ��  � �� �#272B2F6+   �� 5=       � ��
 �����6+   �� 5>       � ��$
 Bold"�6+   �� 5?       � ��$ 14(�6)   �� 5@       � �� �#3336,   �� 5A       � ��$ 0 0 1 0q�60   �� 5B       � ��$ 40��6'   �� 5C       � ��$ 5q�6(   5D      �� 5G   
   "  6   .�5H      ��  � ��$ 5q�6(   �� 5I       � �� �#DDD6,   �� 5J       � ��$ 0 0 1 0q�60   5K      �� 5N   
   " ��6   .�5O      ��  � ��$ Segoe UI)�6+   �� 5P       � ��$ 14(�6)   �� 5Q       � �� �#3336+   5R      5S      .��5U      (�  (� Stretch$ Uniform=�6   .��5V   
   � $	��784��6   $	��720��6   $��10q�6(    � RowDefinitions 5W      �� 5X      $
��Auto �6    �� 5Y      $��* �6    �� 5Z      $
��Auto �6    .+�5^      �� $��0��6    �� Orientation$ 
Horizontal=�6&   $��Center=�6?   $��	0,10,0,10q�6\   .+�5_      �� -   5        $��Kullanıcı Seç��5_      $��99��6R   �� 5`      -   5        $��
Kitap Ekle��5`      6.   5a      �� 5d      $��1��6   $��5,5,5,5q�6&   .+�5e      �� -   5         ��Name$ secilenKullaniciText��5e      $
��Bold"�6:   $��16(�6L   ���#333$��0,0,0,10q�6l     5f      -   5          Name$ kitapDataGrid��5f         AutoGenerateColumns .     CanUserAddRows .  $��0,10,0,0q�6e      Columns 5g      0  *System.Windows.Controls.DataGridTextColumn  5h      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Kitap Adı��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6]   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� *
KitapAdi+6]     5i      $ Yazar��6-   $ * 6V    6<   �� *Yazar+6V   5k      5l      �� 5o      -   5        $��Onayla��5o      $��2��6&   $��Center=�63   $��Center=�6P   $	��100��6k   $��0,10,0,0q�6�   $��36��6�   5p      5q      5r      