   M S B A M L   `   `   `  ���� F  BCoursework1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Coursework1Coursework1      Coursework1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:Coursework1       � Title$	  ELM��$��501.85��$
��1065�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�
����������	#FF506EA2 � Icon$ Res/enveloppe.png��.��� ���	#FF6387C7$��0,0,-6,0q� � ColumnDefinitions �� $
��819* ��� $��21 ��� $
��138* ��� $	��74* �.+��� -    ��Name$ headerTxtBox��$
��Left=�$��23��$��
30,184,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��297��.������  �� Color$ White�� �� Opacity$
 0.75Q�.���� $��Euston Leisure Messaging��$
��Left=�$��	15,10,0,0q�$	��Top=�$	��253��$��20(�$
��Bold"�$��43�����	#FF1F1451�� $��Message Header / ID��$
��Left=�$��
27,153,0,0q�$	��Top=�$��26��$	��121�����	#FF2F2070�� $��Message Body��$
��Left=�$��
30,212,0,0q�$	��Top=�$��87��$��26�����	#FF2F2070�� -   $ messasgeTxtBox��$
��Left=�$	��126��$��
27,243,0,0q�$
 Wrap=�$	��Top=�$	��300��.������ $ White��$
 0.75Q�.���� -   	 ��Name$	 sendBtn��$
��Send��$
��Left=�$��
27,389,0,0q�$	��Top=�$��90��$��37�����	#FF2F2070���	#FF7F96BD���	#FF38404F�� -   $	 clearBtn��$��Clear��$
��Left=�$��128,389,0,0q�$	��Top=�$��90��$��37�����	#FF2F2070���	#FF7F96BD���	#FF38404F�� $"��Significant Incident Reports��$
��Left=�$��
378,18,0,0q�$	��Top=�$��26��$	��183��$��13(�$
��Bold"����	#FF2F2070�� -   
 ��Name$
 
trendsList��$
��Left=�$	��158��$��	24,49,0,0q�$	��Top=�$	��150��$
��Bold"�$��13(�
���   �
�������$��2��$��2������ $ White��$
 0.65Q��� $��Trends��$
��Left=�$��	24,18,0,0q�$	��Top=�$��26��$��60�����	#FF2F2070$��13(�$
��Bold"� J� RenderTransformOrigin$ 0.567,0.577&�$��2���� -   $
 mentionsList��$
��Left=�$	��164��$��
24,262,0,0q�$	��Top=�$	��150��$
��Bold"�$��13(�
�������$��2��$��2������ $ White��$
 0.65Q��� $��Mentions��$
��Left=�$��
24,231,0,0q�$	��Top=�$ 
0.55,0.538&�$��26��$��71�����	#FF2F2070$��13(�$
��Bold"�$��2���� $��URLs Quarantined��$
��Left=�$��378,231,0,0q�$	��Top=�$��26��$	��120�����	#FF2F2070$��13(�$
��Bold"��� $��New Message��$
��Left=�$��
134,94,0,0q�$	��Top=�$��30��$	��103�����	#FF2F2070$
��Bold"�$��14(��� -   $	 
txtFileBtn��$��Open Text File��$
��Left=�$��227,389,0,0q�$	��Top=�$	��100��$��37�����	#FF2F2070���	#FF7F96BD���	#FF38404F&   System.Windows.Controls.DataGrid  -     Name$ 	dataGrid1��$
��Left=�$	��158��$��
378,49,0,0q�$	��Top=�$	��450��   AutoGenerateColumns .     HeadersVisibility$ Column=�
�������   GridLinesVisibility$
 None=�   HorizontalGridLinesBrush
 �   �   
IsReadOnly . $��2������    RowBackground �� $ White��$
 0.35Q����� $ White��$
 0.65Q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Date+  $ Sport Centre Code��$	 120  �� *
CentreCode+  $ Incident Nature��$	 200  �� *
Incident+  -	   $ 	dataGrid2��$
��Left=�$	��164��$��378,262,0,0q�$	��Top=�$	��450��$ Column=� .  $
 None=�
������� . $��2������  �� $ White��$
 0.35Q����� $ White��$
 0.65Q�   $ Sender��$	 190  �� *Sender+  $	 URL��   MinWidth$	 230Q� �� *Url+�� $
��Left=�$��59��$��	40,79,0,0q�$	��Top=�$��77��$��Res/enveloppe.png���� ���	#FF2F2070$
��Left=�$	��472��$��	354,0,0,0q�$	��Top=�$��1��