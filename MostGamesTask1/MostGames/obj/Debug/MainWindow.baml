   M S B A M L   `   `   `  ����D  @MostGames, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MostGames	MostGames  5   	       MostGames.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MostGames     5   	     � Title$  
MainWindow��5   	   $
��1080��6   $
��1920��6*   .��5	      � -   5         �Name$
 Grid��5	       � RowDefinitions 5
   
   �� 5      $	��100��6   $	��100 �6,   �� 5      $��50 �6   $��50��6(   $��50��67   �� 5      $��1* �6    � ColumnDefinitions 5   
   �� 5      $��0.75* �6   �� 5      $��0.25* �6   �� 5      $��0.25* �6   .+�5   
   �� -   5         ��Name$ textbox��5   H   $��150,30,4,40q�6   $��0��6'   $��2��64   $��25��6Y   $��25��6h   �� 5   
   -   5         ��Name$ 
borderTest��5   �   $��2��6   $��1��6"   $��3��6/   $y�Visible=�6C   
���   �$��1q�6m   $��10q�6�   6�   �� 5   
   $��1��6   $
��Text��6   $��0��6-   $��10q�6=   $
��Bold"�6I   $��Center=�6[   �� 5   
   $��1��6   $��
Word count��6   $��1��63   $��10q�6C   $
��Bold"�6O   $��Center=�6a   �� 5   
   $��1��6   $��Vowel count��6   $��2��64   $��10q�6D   $
��Bold"�6P   $��Center=�6b   �� 5   
   -   5        $��0��5      $��2��6   $��
29,30,0,40q�6/   $	��100��6C   $	��100��6R   $
��Left=�6a   $	��102��6|    �� 	IsDefault . $��	Calculate��6�   5      5      