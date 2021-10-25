#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <sys/time.h>
#include <windows.h>

char playground[31][12];

typedef struct pair{
    int f;
    int s;
}pair;
typedef struct klocek{
    pair blok1;
    pair blok2;
    pair blok3;
    pair blok4;
    char type;
    int state;
}klocek;

unsigned long long gettime() {
    //pierwszy raz programuje zegar dokladniejszy niz sekundy, source: https://stackoverflow.com/questions/5833094/get-a-timestamp-in-c-in-microseconds
    struct timeval tv;
    gettimeofday(&tv,NULL);
    return tv.tv_sec*(unsigned long long)1000000+tv.tv_usec;
}
char determine(int rliczba){
    if(rliczba%7==0){
        return 'Z';
    }
    if(rliczba%7==1){
        return 'R';
    }
    if(rliczba%7==2){
        return 'L';
    }
    if(rliczba%7==3){
        return 'T';
    }
    if(rliczba%7==4){
        return 'S';
    }
    if(rliczba%7==5){
        return 'I';
    }
    if(rliczba%7==6){
        return 'B';
    }
}
void setklocek(klocek *blockk,char typ){

    blockk->state=1;//rotacja
    if(typ=='I') {
        blockk->blok1.f = 6;//top x
        blockk->blok1.s = 5;//y
        blockk->blok2.f = 7;//topmid x
        blockk->blok2.s = 5;//y
        blockk->blok3.f = 8;//toplow x
        blockk->blok3.s = 5;//y
        blockk->blok4.f = 9;//bot x
        blockk->blok4.s = 5;//y
        blockk->type = 'I';
    }
    if(typ=='B') {
        blockk->blok1.f = 8;
        blockk->blok1.s = 5;
        blockk->blok2.f = 8;
        blockk->blok2.s = 6;
        blockk->blok3.f = 9;
        blockk->blok3.s = 6;
        blockk->blok4.f = 9;
        blockk->blok4.s = 5;
        blockk->type = 'B';
    }
    if(typ=='L') {
        blockk->blok1.f = 9;
        blockk->blok1.s = 6;
        blockk->blok2.f = 9;
        blockk->blok2.s = 5;
        blockk->blok3.f = 8;
        blockk->blok3.s = 5;
        blockk->blok4.f = 7;
        blockk->blok4.s = 5;
        blockk->type = 'L';
    }
    if(typ=='R') {
        blockk->blok1.f = 9;
        blockk->blok1.s = 4;
        blockk->blok2.f = 9;
        blockk->blok2.s = 5;
        blockk->blok3.f = 8;
        blockk->blok3.s = 5;
        blockk->blok4.f = 7;
        blockk->blok4.s = 5;
        blockk->type = 'R';
    }
    if(typ=='T') {
        blockk->blok1.f = 8;
        blockk->blok1.s = 4;
        blockk->blok2.f = 8;
        blockk->blok2.s = 5;
        blockk->blok3.f = 8;
        blockk->blok3.s = 6;
        blockk->blok4.f = 9;
        blockk->blok4.s = 5;
        blockk->type = 'T';
    }
    if(typ=='Z') {
        blockk->blok1.f = 9;
        blockk->blok1.s = 6;
        blockk->blok2.f = 9;
        blockk->blok2.s = 5;
        blockk->blok3.f = 8;
        blockk->blok3.s = 5;
        blockk->blok4.f = 8;
        blockk->blok4.s = 4;
        blockk->type = 'Z';
    }
    if(typ=='S') {
        blockk->blok1.f = 9;
        blockk->blok1.s = 4;
        blockk->blok2.f = 9;
        blockk->blok2.s = 5;
        blockk->blok3.f = 8;
        blockk->blok3.s = 5;
        blockk->blok4.f = 8;
        blockk->blok4.s = 6;
        blockk->type = 'S';
    }
}
void sidestep(klocek *blockk, int kierunek){
    int plejs=0;
    if(playground[blockk->blok1.f][blockk->blok1.s+kierunek]=='8')
        plejs=1;
    if(playground[blockk->blok2.f][blockk->blok2.s+kierunek]=='8')
        plejs=1;
    if(playground[blockk->blok3.f][blockk->blok3.s+kierunek]=='8')
        plejs=1;
    if(playground[blockk->blok4.f][blockk->blok4.s+kierunek]=='8')
        plejs=1;
    if(plejs==0){
        blockk->blok1.s+=kierunek;
        blockk->blok2.s+=kierunek;
        blockk->blok3.s+=kierunek;
        blockk->blok4.s+=kierunek;
    }
}
void place(klocek *blockk){
    playground[blockk->blok1.f][blockk->blok1.s]='8';
    playground[blockk->blok2.f][blockk->blok2.s]='8';
    playground[blockk->blok3.f][blockk->blok3.s]='8';
    playground[blockk->blok4.f][blockk->blok4.s]='8';
    setklocek(blockk,determine(rand()%10));
}
void move(klocek *blockk){
    int plejs=0;
    if(playground[blockk->blok1.f+1][blockk->blok1.s]=='8')
        plejs=1;
    if(playground[blockk->blok2.f+1][blockk->blok2.s]=='8')
        plejs=1;
    if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
        plejs=1;
    if(playground[blockk->blok4.f+1][blockk->blok4.s]=='8')
        plejs=1;
    if(plejs==1){
        place(blockk);
        return;
    }
    else{
        blockk->blok1.f+=1;
        blockk->blok2.f+=1;
        blockk->blok3.f+=1;
        blockk->blok4.f+=1;
    }
}
void fil(klocek *blockk){
    for(int i=0;i<=11;i++)
        playground[30][i]='8';
    for(int i=0;i<=30;i++)
        playground[i][0]='8';
    for(int i=0;i<=30;i++)
        playground[i][11]='8';
    for(int i=10;i<=29;i++){
        for(int j=1;j<=10;j++){
            if(playground[i][j]!='8')
                playground[i][j]=' ';
        }
    }
    playground[blockk->blok1.f][blockk->blok1.s]='X';
    playground[blockk->blok2.f][blockk->blok2.s]='X';
    playground[blockk->blok3.f][blockk->blok3.s]='X';
    playground[blockk->blok4.f][blockk->blok4.s]='X';

}
void draw(){
    system("cls");
    char ramkapion=186,ramkapoz=205,ramkaroggl=201,ramkaroggp=187,ramkarogdl=200,ramkarogdp=188;
    printf("\n\n\n\n    %c%c%c%c%c%c%c%c%c%c%c%c\n",ramkaroggl,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkaroggp);
    for(int i=10;i<=29;i++){
        printf("    %c",ramkapion);
        for(int j=1;j<=10;j++){
            char pom=219;
            if(playground[i][j]=='8')
                printf("%c",pom);
            else if(playground[i][j]=='X')
                printf("%c",pom);
            else printf("%c",playground[i][j]);
        }
        printf("%c\n",ramkapion);
    }
    printf("    %c%c%c%c%c%c%c%c%c%c%c%c",ramkarogdl,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkapoz,ramkarogdp);
}
void clear(int *lines){
    for(int i=29;i>=10;i--) {
        int count = 0;
        for (int j = 1; j <= 10; j++) {
            if (playground[i][j] == '8')
                count++;
        }
        if (count == 10) {
            *lines+=1;
            for (int j = 1; j <= 10; j++) {
                for (int z = i; z >= 10; z--) {
                    playground[z][j] = playground[z - 1][j];
                }
            }
            i++;
            continue;
        }
    }
}
void rotate(klocek *blockk){
    if(blockk->type=='B')
        return;
    if(blockk->type=='I'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+2]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f;
            blockk->blok1.s = blockk->blok3.s+2;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s+1;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s-1;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='I'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok3.f-2][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f-2;
            blockk->blok1.s = blockk->blok3.s;
            blockk->blok2.f = blockk->blok3.f-1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f+1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=1;
        }
        return;
    }
    if(blockk->type=='Z'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s-1;
            blockk->blok4.f = blockk->blok3.f-1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='Z'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f+1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s-1;
            blockk->state=1;
        }
        return;
    }
    if(blockk->type=='S'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s+1;
            blockk->blok4.f = blockk->blok3.f-1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='S'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f+1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s+1;
            blockk->state=1;
        }
        return;
    }
    if(blockk->type=='T'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok2.f-1][blockk->blok2.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok3.f = blockk->blok2.f-1;
            blockk->blok3.s = blockk->blok2.s;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='T'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok2.f][blockk->blok2.s+1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok4.f = blockk->blok2.f;
            blockk->blok4.s = blockk->blok2.s+1;
            blockk->state=3;
        }
        return;
    }
    if(blockk->type=='T'&&blockk->state==3){
        int plejs=0;
        if(playground[blockk->blok2.f+1][blockk->blok2.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok2.f+1;
            blockk->blok1.s = blockk->blok2.s;
            blockk->state=4;
        }
        return;
    }
    if(blockk->type=='T'&&blockk->state==4){
        int plejs=0;
        if(playground[blockk->blok2.f][blockk->blok2.s-1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok2.f;
            blockk->blok1.s = blockk->blok2.s-1;
            blockk->blok3.f = blockk->blok2.f;
            blockk->blok3.s = blockk->blok2.s+1;
            blockk->blok4.f = blockk->blok2.f+1;
            blockk->blok4.s = blockk->blok2.s;
            blockk->state=1;
        }
        return;
    }
    if(blockk->type=='L'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s-1;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s+1;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='L'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok3.f-1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f-1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f-1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f+1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=3;
        }
        return;
    }
    if(blockk->type=='L'&&blockk->state==3){
        int plejs=0;
        if(playground[blockk->blok3.f-1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f-1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s+1;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s-1;
            blockk->state=4;
        }
        return;
    }
    if(blockk->type=='L'&&blockk->state==4){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f+1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f-1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=1;
        }
        return;
    }
    if(blockk->type=='R'&&blockk->state==1){
        int plejs=0;
        if(playground[blockk->blok3.f-1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f-1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s-1;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s+1;
            blockk->state=2;
        }
        return;
    }
    if(blockk->type=='R'&&blockk->state==2){
        int plejs=0;
        if(playground[blockk->blok3.f-1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f-1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f-1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f+1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=3;
        }
        return;
    }
    if(blockk->type=='R'&&blockk->state==3){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s+1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f][blockk->blok3.s-1]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s+1;
            blockk->blok2.f = blockk->blok3.f;
            blockk->blok2.s = blockk->blok3.s+1;
            blockk->blok4.f = blockk->blok3.f;
            blockk->blok4.s = blockk->blok3.s-1;
            blockk->state=4;
        }
        return;
    }
    if(blockk->type=='R'&&blockk->state==4){
        int plejs=0;
        if(playground[blockk->blok3.f+1][blockk->blok3.s-1]=='8')
            plejs=1;
        if(playground[blockk->blok3.f+1][blockk->blok3.s]=='8')
            plejs=1;
        if(playground[blockk->blok3.f-1][blockk->blok3.s]=='8')
            plejs=1;
        if(plejs==0){
            blockk->blok1.f = blockk->blok3.f+1;
            blockk->blok1.s = blockk->blok3.s-1;
            blockk->blok2.f = blockk->blok3.f+1;
            blockk->blok2.s = blockk->blok3.s;
            blockk->blok4.f = blockk->blok3.f-1;
            blockk->blok4.s = blockk->blok3.s;
            blockk->state=1;
        }
        return;
    }
}
int gameover(){
    int count=0;
    for(int i=1;i<=10;i++)
        if(playground[9][i]=='8')
            count++;
    if(count>0)
        return 1;
    return 0;
}
void endgame(int *lines){
    system("CLS");
    printf("\n\n\n\n\n     Game  Over\n     Score:%d\n\n     Save score?\n         Y/N ",*lines);
}
int main() {
    ///////////////////////////////////////////////////////////////////////////////////
    //ustawienia konsoli konsoli
    SetConsoleTitle("Ctris");
    CONSOLE_FONT_INFOEX font={sizeof(font)};
    GetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE),TRUE,&font);
    font.dwFontSize.X=16;
    font.dwFontSize.Y=16;
    wcscpy(font.FaceName, L"Terminal");
    SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE),TRUE,&font);
    SMALL_RECT wsize={0,0,20,30};
    SetConsoleWindowInfo(GetStdHandle(STD_OUTPUT_HANDLE),1,&wsize);
    COORD bsize={20,30};
    SetConsoleScreenBufferSize(GetStdHandle(STD_OUTPUT_HANDLE),bsize);
    srand(time(NULL));
    ////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////
    klocek blockk;
    int score=0;
    setklocek(&blockk,determine(rand()%10));
    while(gameover()!=1) {
        char cc = 0;
        unsigned long long curtime=gettime();
        //Sleep(500); prototyp
        while(gettime()-curtime<300000){
            cc=0;
            if (kbhit())
            cc = getch();
        // nie lubie switch
        /*if (cc == 'p') {
            place(&blockk);
            fil(&blockk);
            draw();
        }*/
        /*if (cc == 'k') {
            scanf("%c", &cc);
            setklocek(&blockk, cc);
            fil(&blockk);
            draw();
            //cheat do sprawdzania programu
        }*/
        if (cc == 's'|| cc=='S') {
            move(&blockk);
            clear(&score);
            fil(&blockk);
            draw();
        }
        if (cc == 'a'|| cc=='A') {
            sidestep(&blockk, -1);
            fil(&blockk);
            draw();
        }
        if (cc == 'd'|| cc=='D') {
            sidestep(&blockk, 1);
            fil(&blockk);
            draw();
        }
        if (cc == 'w'|| cc=='W') {
            rotate(&blockk);
            fil(&blockk);
            draw();
        }
    }
        move(&blockk);
        clear(&score);
        fil(&blockk);
        draw();
    }
    endgame(&score);
    char c;
    scanf("%c",&c);
    if(c=='y'||c=='Y') {
        FILE* savehere=fopen("save.txt","a");
        char string[1000];
        int size=0,tak=0;

        while(1) {
            scanf("%c",&string[size]);
            if(string[size]!=10)
                tak=1;
            if(tak==1)
                fprintf(savehere,"%c",string[size]);
            if(string[size]==10&&tak==1)
                break;
            size++;
        }
        fprintf(savehere,"%d\n",score);
        fclose(savehere);
    }
    return 0;
}