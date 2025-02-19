import React from 'react'



//אודות הפרויקט ומדריך למשתמש
export default function Aboat() {
    return (
        <p dir='rtl' style={{ fontSize: "11.9px" }} >
            <b>תיאור הפרויקט:</b>
            <br></br>

            התוכנה נועדה להקל בקריאת מיילים רבים בנושאים שונים כאשר היא מסווגת את המייל לקטגוריה אחרי ניתוח הטקסט ומציגה למשתמש את התוכן העיקרי של המייל שקשור לאותה קטגוריה.
            התוכנה מתבססת בעיקר על ניתוח והבנת הטקסט. היא מתנהלת באופן דינאמי כך שכל
            משתמש יכול להוסיף נושאים למיון ולהסיר נושאים לא רצויים ולצורך כך התוכנה הינה תוכנה מתלמדת.
            <br></br>
            <br></br>
            <br></br>

            <b>מדריך למשתמש:
            </b>
            <br></br>

            <u>"LOGIN"</u>

            <br></br>

            על המשתמש להירשם למערכת או לבצע התחברות לחשבון קיים.
            <br></br>
            בעת הרשמה/ התחברות המשתמש נדרש להכניס כתובת אימייל וסיסמא אישית, ברגע שהמשתמש לוחץ על כפתור "כניסה" המערכת בודקת האם חשבון המייל כבר נמצא אצלה,
            אם כן המשתמש מועבר לדף הבא "הצגת מיילים".
            <br></br>
            אם המשתמש אינו קיים, המערכת שואלת אותו אם ליצור בשבילו חשבון חדש ואם הוא מעוניין היא שולחת אליו מייל עם קוד אשר אותו צריך להזין בשביל אימות המייל (מטעמי אבטחה), ברגע שהמשתמש מזין את הקוד הנכון פרטיו נשמרים במערכת והוא מועבר לדף הבא.
            המייל שנשלח ע"י המערכת:
            <br></br>
            <u>"הצגת מיילים"
            </u>
            <br></br>

            לדף זה מועבר המשתמש אחרי אימות וכניסה למערכת.
            <br></br>

            בצד הימני של דף זה נמצאים הכפתורים המנווטים לכל דף באתר, ובמרכזו יתחלף התוכן לפי המיקום הנוכחי של המשתמש.
            דף זה נפתח אוטומטית עם הטבלת הדואר הנכנס של המשתמש, כל שורה בטבלה מייצגת מייל אחר ופרטים עליו כמו שם השולח באיזה תאריך המייל הגיע וכו'.
            לכל מייל ישנה תווית המסמלת את קטגורית המייל, לחיצה על תווית של קטגוריה x תוביל את המשתמש לדף הבא- "הצגת מיילים לפי קטגורית x".
            לכל מייל מופיע גם משפט מתוך המייל שהכי קשור לקטגוריה שלו.
            בנוסף בצד השמאלי של השורה נמצא אייקון שבלחיצה עליו מופיעות כל הקטגוריות של המשתמש (מלבד קטגורית המייל הנוכחי), בלחיצה על קטגוריה מסוימת המערכת משנה את קטגורית המייל לקטגוריה הרצויה.

            <br></br>
            <u>"הצגת מיילים לפי קטגוריה"
            </u>
            <br></br>

            בדף זה מוצגים רק המיילים ששייכים לקטגוריה מסוימת.

            <br></br>
            בלחיצה על כפתור "קטגוריות" אשר נמצא בצד הימני של המסך מוצגות כל הקטגוריות של המשתמש, בלחיצה על קטגוריה מוצגים כל המיילים של אותה קטגוריה.
            ניתן להגיע לדף זה ע"י לחיצה על תוויות הקטגוריות אשר נמצאות בדפים "הצגת קטגוריות" ו- "הצגת מיילים".
            <br></br>
            <u>"ניהול קטגוריות"
            </u>
            <br></br>

            לדף זה ניתן להגיע ע"י לחיצה על הכפתור "ניהול קטגוריות" הנמצא בצד הימני של המסך.

            <br></br>

            דף זה מתחלק ל-2:
            <br></br>

            •	בצד העליון מוצגת טבלה בה מופיעות כל הקטגוריות שהמשתמש הגדיר עד כה.
            כל שורה מייצגת קטגוריה אחרת ופרטים על אותה קטגוריה כמו נושא אב (אם קיים) ותווית הנושא (שלחיצה עליה מובילה לדף "הצגת מיילים לפי קטגוריות").
            עבור כל קטגוריה ישנה אופציה לשינוי צבע התווית שלה בלחיצה על האייקון המוצג בעמודת "שינוי צבע", ובנוסף אופציה למחיקת הקטגוריה- ע"י לחיצה על האייקון השמאלי ביותר.
            <br></br>

            •	מתחת לטבלה נתנה אפשרות למשתמש להגדיר קטגוריות חדשות, על המשתמש להכניס את שם הנושא, שם נושא האב (אם רוצים שהנושא החדש יהיה תת נושא של נושא מסוים) וצבע לתוויית, בלחיצה על כפתור הפלוס פרטי הקטגוריה נשמרים במערכת והיא מתווספת לטבלה.
            <br></br>
            <u>"אודות"
            </u>
            <br></br>

            לדף זה ניתן להגיע ע"י לחיצה על הכפתור "אודות" הנמצא בצד הימני של המסך.
            בדף זה מוצגים פרטים אודות המערכת ומדריך למשתמש.


            <br></br>
            <br></br>
            <br></br>
            <br></br>



        </p>
    )
}
