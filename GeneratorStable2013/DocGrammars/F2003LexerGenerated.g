lexer  grammar F2003LexerGenerated;

options
{
  language = C;
}

@lexer::members
{
#include "FParser.h"
using namespace OPS::Fortran2003Parser;
#define LEXER_BASE ((FortranLexerBase*)ctx->pLexer->super) 
}

T_EOS 
  : ';' 
  ;
  
T_NEWLINE
  :  '\r'? '\n'    
  ;
      
CONTINUE_CHAR 
  : '&' 
    {         
      $channel = HIDDEN;
    };


// R427 from char-literal-constant
T_CHAR_CONSTANT
  : ('\'' ( SQ_Rep_Char )* '\'')+ 
  | ('\"' ( DQ_Rep_Char )* '\"')+ 
  ;

T_DIGIT_STRING
  :  Digit_String 
  ;

// R412
BINARY_CONSTANT
  : ('b'|'B') '\'' ('0'..'1')+ '\''
  | ('b'|'B') '\"' ('0'..'1')+ '\"'
  ;

// R413
OCTAL_CONSTANT
    : ('o'|'O') '\'' ('0'..'7')+ '\''
    | ('o'|'O') '\"' ('0'..'7')+ '\"'
    ;

// R414
HEX_CONSTANT
    : ('z'|'Z') '\'' (Digit|'a'..'f'|'A'..'F')+ '\''
    | ('z'|'Z') '\"' (Digit|'a'..'f'|'A'..'F')+ '\"'
    ;


WS  :  (' '|'\t') {
            $channel = HIDDEN;
       }
    ;

/*
 * fragments
 */

// R409 digit_string
fragment
Digit_String : Digit+  ;


// R302 alphanumeric_character
fragment
Alphanumeric_Character : Letter | Digit | '_' ;

fragment
Special_Character
    :    ' ' .. '/' 
    |    ':' .. '@' 
    |    '[' .. '^' 
    |    '`' 
    |    '{' .. '~' 
    ;

fragment
Rep_Char : ~('\'' | '\"') ;

fragment
SQ_Rep_Char : ~('\'') ;
fragment
DQ_Rep_Char : ~('\"') ;

fragment
Letter : ('a'..'z' | 'A'..'Z') ;

fragment
Digit : '0'..'9'  ;

PREPROCESS_LINE : '#' ~('\n'|'\r')*  {
            $channel = HIDDEN;
        } ;

T_INCLUDE 
options {k=1;} : 'INCLUDE' {
        };


/*
 * from fortran03_lexer.g
 */

T_ASTERISK      : '*'   ;
T_COLON         : ':'   ;
T_COLON_COLON   : '::'  ;
T_COMMA         : ','   ;
T_EQUALS        : '='   ;
T_EQ_EQ         : '=='  ;
T_EQ_GT         : '=>'  ;
T_GREATERTHAN   : '>'   ;
T_GREATERTHAN_EQ: '>='  ;
T_LESSTHAN      : '<'   ;
T_LESSTHAN_EQ   : '<='  ;
T_LBRACKET      : '['   ;
T_LPAREN        : '('   ;
T_MINUS         : '-'   ;
T_PERCENT       : '%'   ;
T_PLUS          : '+'   ;
T_POWER         : '**'  ;
T_SLASH         : '/'   ;
T_SLASH_EQ      : '/='  ;
T_SLASH_SLASH   : '//'   ;
T_RBRACKET      : ']'   ;
T_RPAREN        : ')'   ;
T_UNDERSCORE    : '_'   ;

T_EQ            : '.EQ.' ;
T_NE            : '.NE.' ;
T_LT            : '.LT.' ;
T_LE            : '.LE.' ;
T_GT            : '.GT.' ;
T_GE            : '.GE.' ;

T_TRUE          : '.TRUE.'  ;
T_FALSE         : '.FALSE.' ;

T_NOT           : '.NOT.' ;
T_AND           : '.AND.' ;
T_OR            : '.OR.'  ;
T_EQV           : '.EQV.' ;
T_NEQV          : '.NEQV.';

T_PERIOD_EXPONENT 
    : '.' ('0'..'9')+ ('E' | 'e' | 'd' | 'D') ('+' | '-')? ('0'..'9')+  
    | '.' ('E' | 'e' | 'd' | 'D') ('+' | '-')? ('0'..'9')+  
    | '.' ('0'..'9')+
    | ('0'..'9')+ ('e' | 'E' | 'd' | 'D') ('+' | '-')? ('0'..'9')+ 
    ;

T_PERIOD        : '.' ;




T_INTEGER       :       'INTEGER'       ;
T_REAL          :       'REAL'          ;
T_COMPLEX       :       'COMPLEX'       ;
T_CHARACTER     :       'CHARACTER'     ;
T_LOGICAL       :       'LOGICAL'       ;

T_ABSTRACT      :       'ABSTRACT'      ;
T_ALLOCATABLE   :       'ALLOCATABLE'   ;
T_ALLOCATE      :       'ALLOCATE'      ;
T_ASSIGNMENT    :       'ASSIGNMENT'    ;
// ASSIGN statements are a deleted feature.
T_ASSIGN        :       'ASSIGN'        ;
T_ASSOCIATE     :       'ASSOCIATE'     ;
T_ASYNCHRONOUS  :       'ASYNCHRONOUS'  ;
T_BACKSPACE     :       'BACKSPACE'     ;
T_BLOCK         :       'BLOCK'         ;
T_BLOCKDATA     :       'BLOCKDATA'     ;
T_CALL          :       'CALL'          ;
T_CASE          :       'CASE'          ;
T_CLASS         :       'CLASS'         ;
T_CLOSE         :       'CLOSE'         ;
T_COMMON        :       'COMMON'        ;
T_CONTAINS      :       'CONTAINS'      ;
T_CONTINUE      :       'CONTINUE'      ;
T_CYCLE         :       'CYCLE'         ;
T_DATA          :       'DATA'          ;
T_DEFAULT       :       'DEFAULT'       ;
T_DEALLOCATE    :       'DEALLOCATE'    ;
T_DEFERRED      :       'DEFERRED'      ;
T_DO            :       'DO'            ;
T_DOUBLE        :       'DOUBLE'        ;
T_DOUBLEPRECISION:      'DOUBLEPRECISION' ;
T_DOUBLECOMPLEX:        'DOUBLECOMPLEX' ;
T_ELEMENTAL     :       'ELEMENTAL'     ;
T_ELSE          :       'ELSE'          ;
T_ELSEIF        :       'ELSEIF'        ;
T_ELSEWHERE     :       'ELSEWHERE'     ;
T_ENTRY         :       'ENTRY'         ;
T_ENUM          :       'ENUM'          ;
T_ENUMERATOR    :       'ENUMERATOR'    ;
T_EQUIVALENCE   :       'EQUIVALENCE'   ;
T_EXIT          :       'EXIT'          ;
T_EXTENDS       :       'EXTENDS'       ;
T_EXTERNAL      :       'EXTERNAL'      ;
T_FILE          :       'FILE'          ;
T_FINAL         :       'FINAL'         ;
T_FLUSH         :       'FLUSH'         ;
T_FORALL        :       'FORALL'        ;
T_FORMAT        :       'FORMAT'        ;
T_FORMATTED     :       'FORMATTED'     ;
T_FUNCTION      :       'FUNCTION'      ;
T_GENERIC       :       'GENERIC'       ;
T_GO            :       'GO'            ;
T_GOTO          :       'GOTO'          ;
T_IF            :       'IF'            ;
T_IMPLICIT      :       'IMPLICIT'      ;
T_IMPORT        :       'IMPORT'        ;
T_IN            :       'IN'            ;
T_INOUT         :       'INOUT'         ;
T_INTENT        :       'INTENT'        ;
T_INTERFACE     :       'INTERFACE'     ;
T_INTRINSIC     :       'INTRINSIC'     ;
T_INQUIRE       :       'INQUIRE'       ;
T_MODULE        :       'MODULE'        ;
T_NAMELIST      :       'NAMELIST'      ;
T_NONE          :       'NONE'          ;
T_NON_INTRINSIC :       'NON_INTRINSIC' ;
T_NON_OVERRIDABLE:      'NON_OVERRIDABLE';
T_NOPASS        :       'NOPASS'        ;
T_NULLIFY       :       'NULLIFY'       ;
T_ONLY          :       'ONLY'          ;
T_OPEN          :       'OPEN'          ;
T_OPERATOR      :       'OPERATOR'      ;
T_OPTIONAL      :       'OPTIONAL'      ;
T_OUT           :       'OUT'           ;
T_PARAMETER     :       'PARAMETER'     ;
T_PASS          :       'PASS'          ;
T_PAUSE         :       'PAUSE'         ;
T_POINTER       :       'POINTER'       ;
T_PRINT         :       'PRINT'         ;
T_PRECISION     :       'PRECISION'     ;
T_PRIVATE       :       'PRIVATE'       ;
T_PROCEDURE     :       'PROCEDURE'     ;
T_PROGRAM       :       'PROGRAM'       ;
T_PROTECTED     :       'PROTECTED'     ;
T_PUBLIC        :       'PUBLIC'        ;
T_PURE          :       'PURE'          ;
T_READ          :       'READ'          ;
T_RECURSIVE     :       'RECURSIVE'     ;
T_RESULT        :       'RESULT'        ;
T_RETURN        :       'RETURN'        ;
T_REWIND        :       'REWIND'        ;
T_SAVE          :       'SAVE'          ;
T_SELECT        :       'SELECT'        ;
T_SELECTCASE    :       'SELECTCASE'    ;
T_SELECTTYPE    :       'SELECTTYPE'    ;
T_SEQUENCE      :       'SEQUENCE'      ;
T_STOP          :       'STOP'          ;
T_SUBROUTINE    :       'SUBROUTINE'    ;
T_TARGET        :       'TARGET'        ;
T_THEN          :       'THEN'          ;
T_TO            :       'TO'            ;
T_TYPE          :       'TYPE'          ;
T_UNFORMATTED   :       'UNFORMATTED'   ;
T_USE           :       'USE'           ;
T_VALUE         :       'VALUE'         ;
T_VOLATILE      :       'VOLATILE'      ;
T_WAIT          :       'WAIT'          ;
T_WHERE         :       'WHERE'         ;
T_WHILE         :       'WHILE'         ;
T_WRITE         :       'WRITE'         ;

T_ENDASSOCIATE  :       'ENDASSOCIATE'  ;
T_ENDBLOCK      :       'ENDBLOCK'      ;
T_ENDBLOCKDATA  :       'ENDBLOCKDATA'  ;
T_ENDDO         :       'ENDDO'         ;
T_ENDENUM       :       'ENDENUM'       ;
T_ENDFORALL     :       'ENDFORALL'     ;
T_ENDFILE       :       'ENDFILE'       ;
T_ENDFUNCTION   :       'ENDFUNCTION'   ;
T_ENDIF         :       'ENDIF'         ;
T_ENDINTERFACE  :       'ENDINTERFACE'  ;
T_ENDMODULE     :       'ENDMODULE'     ;
T_ENDPROGRAM    :       'ENDPROGRAM'    ;
T_ENDSELECT     :       'ENDSELECT'     ;
T_ENDSUBROUTINE :       'ENDSUBROUTINE' ;
T_ENDTYPE       :       'ENDTYPE'       ;
T_ENDWHERE      :       'ENDWHERE'      ;

T_END   : 'END'
        ;

T_DIMENSION     :       'DIMENSION'     ;

T_KIND : 'KIND' ;
T_LEN : 'LEN' ;

T_BIND : 'BIND' ;

T_HOLLERITH : Digit_String 'H' ;

// Must come after .EQ. (for example) or will get matched first
// TODO:: this may have to be done in the parser w/ a rule such as:
// T_PERIOD T_IDENT T_PERIOD
T_DEFINED_OP
    :    '.' Letter+ '.'
    ;

// // used to catch edit descriptors and other situations
// T_ID_OR_OTHER
// 	:	'ID_OR_OTHER'
// 	;

// extra, context-sensitive terminals that require communication between parser and scanner
// added the underscores so there is no way this could overlap w/ any valid
// idents in Fortran.  we just need this token to be defined so we can 
// create one of them while we're fixing up labeled do stmts.
T_LABEL_DO_TERMINAL
	:	'__LABEL_DO_TERMINAL__'
	;

T_DATA_EDIT_DESC : '__T_DATA_EDIT_DESC__' ;
T_CONTROL_EDIT_DESC : '__T_CONTROL_EDIT_DESC__' ;
T_CHAR_STRING_EDIT_DESC : '__T_CHAR_STRING_EDIT_DESC__' ;

T_STMT_FUNCTION 
	:	'STMT_FUNCTION'
	;

T_ASSIGNMENT_STMT : '__T_ASSIGNMENT_STMT__' ;
T_PTR_ASSIGNMENT_STMT : '__T_PTR_ASSIGNMENT_STMT__' ;
T_ARITHMETIC_IF_STMT : '__T_ARITHMETIC_IF_STMT__' ;
T_ALLOCATE_STMT_1 : '__T_ALLOCATE_STMT_1__' ;
T_WHERE_STMT : '__T_WHERE_STMT__' ;
T_IF_STMT : '__T_IF_STMT__' ;
T_FORALL_STMT : '__T_FORALL_STMT__' ;
T_WHERE_CONSTRUCT_STMT : '__T_WHERE_CONSTRUCT_STMT__' ;
T_FORALL_CONSTRUCT_STMT : '__T_FORALL_CONSTRUCT_STMT__' ;
T_INQUIRE_STMT_2 : '__T_INQUIRE_STMT_2__' ;
// text for the real constant will be set when a token of this type is 
// created by the prepass.
T_REAL_CONSTANT : '__T_REAL_CONSTANT__' ; 

T_INCLUDE_NAME: '__T_INCLUDE_NAME__' ;
T_EOF: '__T_EOF__' ;

// R304
T_IDENT
options {k=1;}
	:	Letter ( Alphanumeric_Character )*
	;

LINE_COMMENT
    : '!'  ~('\n'|'\r')*  
        {
            $channel = HIDDEN;
        }
    ;

/* Need a catch-all rule because of fixed-form being allowed to use any 
   character in column 6 to designate a continuation.  */
MISC_CHAR : ~('\n' | '\r') ;




