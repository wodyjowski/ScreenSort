   M S B A M L   `   `   `  ���� E  AScreenSort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ScreenSort
ScreenSort      ScreenSort.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:ScreenSort       � Title$  
ScreenSort��$	��475��$	��815��$	��800��$	��450�����	#FF3C3C3C � Icon$ Resources/icon.ico�� � ContextMenu ��  J� 	AllowDrop . .@�m� -   $��Random��.���  � RowDefinitions �� $
��379* ��� $	��40* �.+��� -    ��Name$ screenshotButton��$��
Screenshot��$��	0,0,10,10q�$��1��$��20��$��Bottom=�$��Right=�$��75���� -    ��Name$ previewImage������ *WriteableBm+�� -   $ 
SortButton��{�.  $
��Sort��$��	0,0,90,10q�$��1��$��Right=�$��75��$��20��$��Bottom=��� -    ��Name$ comboBoxSortType��$��	0,0,170,8q�$��1��$��24��$��Bottom=�$��Right=�$	��200���� -    ��Name$ 	labelTime��$��Ready��$��
10,0,621,7q�$��1��	 J� RenderTransformOrigin$	 0.465,0.885&�$��26��$��Bottom=����	#FFF5D231�� -   
 ��Name$
 textBoxResolution��$��	0,0,375,8q�$��1�� �� TextWrapping$
 Wrap=�$��	1920x1080��$��23��$��Bottom=�$��Right=�$��70���� -   $
 textBoxDelay��$��	0,0,450,8q�$��1��$
 Wrap=�$��0��$��23��$��Bottom=�$��Right=�$��70�����	#FF950000�� -	    ��Name$ checkBoxHSB��$	��HSB��$��Right=�$��
0,0,525,12q�$��1�����	#FFD33FFF$��15��$��Bottom=�