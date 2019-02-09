#F700:     uni0E10.descless    (base.descless)
Select("thoThanthai.less");
Copy();
Select("U+f700");
Paste();

#F701~04:  uni0E34~37.left     (upper.left)
google = ["uni0E34.narrow","uni0E35.narrow","uni0E36.narrow","uni0E37.narrow"];
nectec = ["U+f701","U+f702","U+f703","U+f704"];
i=0;
while(i<4)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i = i+1;
endloop

#F705~09:  uni0E48~4C.lowleft  (top.lowleft)
google = ["uni0E48.narrow","uni0E49.narrow","uni0E4A.narrow","uni0E4B.narrow","uni0E4C.narrow"];
nectec = ["U+f705","U+f706","U+f707","U+f708","U+f709"];
i=0;
while(i<5)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop

#F70A~0E:  uni0E48~4C.low      (top.low)
google = ["uni0E48","uni0E49","uni0E4A","uni0E4B","uni0E4C"];
nectec = ["U+f70A","U+f70B","U+f70C","U+f70D","U+f70E"];
i=0;
while(i<5)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop

#F70F:     uni0E0D.descless    (base.descless)
Select("yoYingthai.less");
Copy();
Select("U+f70F");
Paste();

#F710~12:  uni0E31,4D,47.left  (upper.left)
google = ["uni0E31.narrow","uni0E4D.narrow","uni0E47.narrow"];
nectec = ["U+f710","U+f711","U+f712"];
i=0;
while(i<3)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop

#F713~17:  uni0E48~4C.left     (top.left)
google = ["uni0E48.small","uni0E49.small","uni0E4A.small","uni0E4B.small","uni0E4C.small"];
nectec = ["U+f713","U+f714","U+f715","U+f716","U+f717"];
i=0;
while(i<5)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop

#Replace Original with small:  uni0E48~4C.low      (top.low)
google = ["uni0E48.small","uni0E49.small","uni0E4A.small","uni0E4B.small","uni0E4C.small"];
nectec = ["U+0e48","U+0e49","U+0e4A","U+0e4B","U+0e4C"];
i=0;
while(i<5)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop

#F718~1A:  uni0E38~3A.low      (lower.low)
google = ["uni0E38.small","uni0E39.small","uni0E3A.small"];
nectec = ["U+f718","U+f719","U+f71A"];
i=0;
while(i<3)
    Select(google[i]);
    Copy();
    Select(nectec[i]);
    Paste();
    i=i+1;
endloop
