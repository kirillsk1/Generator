parser grammar F2003ParserGenerated;

options
{
  language = C;
  output = AST;
  tokenVocab = F2003LexerGenerated;
}

tokens
{
  T_MAIN_PROGRAM;
  
  T_SPECIFICATION_PART;  
  T_EXECUTION_PART;

//4.4
  T_SECTION_SUBSCRIPT_LIST;
  T_INTRINSIC_TYPE_SPEC;
  T_KIND_SELECTOR;
  
  T_SIGNED_REAL_LITERAL_CONSTANT;
  T_CHAR_LITERAL_CONSTANT;
  
  T_CHAR_SELECTOR;
  T_CHAR_SELECTOR_ASTERISK;
  T_CHAR_SELECTOR_3_VARIANT;
  
  T_AC_VALUE_LIST;
  T_LENGTH_SELECTOR_1;
  T_LENGTH_SELECTOR_2;
//4,7
  T_ARRAY_CONSTRUCTOR;
  T_AC_INPLIED_DO;
  T_AC_IMPLIED_DO_CONTROL;
  T_AC_SPEC;
    
// 5    
  T_TYPE_DECLARATION_STMT;
  T_ENTITY_DECL;
  T_ARRAY_SPEC_ELEMENT;

// 6
  T_DEGIGNATOR;
  T_SUBSTRING_RANGE;
  T_DATA_REF;
  T_PART_REF;
  T_SECTION_SUBSCRIPT;
    
  T_DEFFERED_SHAPE_SYMBOL;  
  T_ASSIGNMENT_STMT;
  T_DEFINED_BINARY_OP;
  
  T_EMPTY_FIELD;
  T_ARRAY_SPEC;
  
  T_DERIVED_TYPE_DEF;
  T_DATA_COMPONENT_DEF_STMT;
  T_STRUCTURE_CTOR;
  T_DERIVED_TYPE_STMT;
  T_TYPE_PARAM_SPEC;
  T_DATA_COMP_DEF_STMT;
  T_COMP_DECL;
  T_IF_CONSTRUCT;
  T_IF_THEN;
  T_DO_CONSTRUCT;
  T_FOR_LOOP_CONTROL;
  T_DO_TERM_ACTION_STMT;
}

// $<Section 2

// R201
program
  :   T_NEWLINE* (program_unit)* -> program_unit*
  ;

// R202
program_unit
  :  main_program 
//|  external_subprogram
//|  module
//|  block_data
  ;

// R204
specification_part
  :	
//( use_stmt {numUseStmts++;})*
//( import_stmt {numImportStmts++;})*
    declaration_construct* -> ^(T_SPECIFICATION_PART declaration_construct*)
  ;

// R207
declaration_construct
  :
     type_declaration_stmt
  |  derived_type_def
//   entry_stmt
//|  parameter_stmt
//|  format_stmt
//|  implicit_stmt  
//|  implicit_stmt must precede all occurences of the below
//
//|  enum_def
//|  interface_block
//|  procedure_declaration_stmt
//|  specification_stmt     
//|  stmt_function_stmt
  ;

// R208
execution_part
  :  execution_part_construct* -> ^(T_EXECUTION_PART execution_part_construct*)
  ;

// R209
execution_part_construct
  :  {true}? => label? executable_construct    
  ;
	
// R213
executable_construct
  :  action_stmt
  |  do_construct
  |  if_construct
//	|	associate_construct
//	|	case_construct
//	|	forall_construct
//	|	select_type_construct
//	|	where_construct
  ;
	
// R214
action_stmt
  :  assignment_stmt
  |  if_stmt
  |  cycle_stmt
  |  exit_stmt
//  |	if_stmt
  //	allocate_stmt
/*	|	backspace_stmt
	|	call_stmt
	|	close_stmt
	|	continue_stmt
	|	deallocate_stmt
	|	endfile_stmt

	|	flush_stmt
	|	forall_stmt
	|	goto_stmt
	|	if_stmt
    |   inquire_stmt  
	|	nullify_stmt
	|	open_stmt
	|	pointer_assignment_stmt
	|	print_stmt
	|	read_stmt
	|	return_stmt
	|	rewind_stmt
	|	stop_stmt
	|	wait_stmt
	|	where_stmt
	|	write_stmt
	|	arithmetic_if_stmt
	|	computed_goto_stmt
    |   assign_stmt 
    |   assigned_goto_stmt
    |   pause_stmt*/
  ;	

// $>

// $<section 3

// R304
name 
  :  T_IDENT
  ;
	
	
// $<3.2.2 constants

// R305
constant
  :  literal_constant
  |  T_IDENT
  ;
	
// R306
literal_constant
  :  int_literal_constant
  |  char_literal_constant
  |  real_literal_constant
  ;



// R308
int_constant
  :  int_literal_constant
  |  T_IDENT
  ;

// R309
char_constant
  :  char_literal_constant
  |  T_IDENT
  ;
  
// $>

// $>

// $>

// R313
label 
  :  T_DIGIT_STRING
  ;

// $<section 4

// R401
type_spec
  :  intrinsic_type_spec
|//	derived_type_spec
  ;    

// R402
type_param_value
  :  expr
  |  T_ASTERISK
  |  T_COLON
  ;


// $<4.4 intrinsic types

// R403
intrinsic_type_spec
  :  T_INTEGER kind_selector?			-> ^(T_INTRINSIC_TYPE_SPEC T_INTEGER kind_selector?)
  |  T_REAL kind_selector? 			-> ^(T_INTRINSIC_TYPE_SPEC T_REAL kind_selector?)
  |  T_DOUBLE T_PRECISION | T_DOUBLEPRECISION 	-> ^(T_INTRINSIC_TYPE_SPEC T_DOUBLEPRECISION)
  |  T_COMPLEX kind_selector?			-> ^(T_INTRINSIC_TYPE_SPEC T_COMPLEX kind_selector?)
  |  T_DOUBLE T_COMPLEX | T_DOUBLECOMPLEX 	-> ^(T_INTRINSIC_TYPE_SPEC T_DOUBLECOMPLEX)
  |  T_CHARACTER char_selector? 		-> ^(T_INTRINSIC_TYPE_SPEC T_CHARACTER char_selector?)
  |  T_LOGICAL kind_selector?			-> ^(T_INTRINSIC_TYPE_SPEC T_LOGICAL kind_selector?)
  ;

// R404
kind_selector
  :  T_LPAREN (T_KIND T_EQUALS)? expr T_RPAREN 	-> ^(T_KIND_SELECTOR expr)
  |  T_ASTERISK T_DIGIT_STRING 			-> ^(T_KIND_SELECTOR T_DIGIT_STRING) 
  ;  // need to delete second subtree

// $<4.4.1 Integer type

// R405
signed_int_literal_constant
  :  T_PLUS? int_literal_constant -> int_literal_constant
  |  T_MINUS int_literal_constant ->^(T_MINUS int_literal_constant)
  ;

// R406
int_literal_constant
  :  T_DIGIT_STRING (T_UNDERSCORE kind_param)? -> ^(T_DIGIT_STRING kind_param?)
  ;

// R407
kind_param
  :  T_DIGIT_STRING 
  |  T_IDENT 
  ;
	


// $>


// $<4.4.2 Real Type

// R416
signed_real_literal_constant
  :  T_PLUS? real_literal_constant -> real_literal_constant
  |  T_MINUS real_literal_constant ->^(T_MINUS real_literal_constant)
  ;


// R417 
real_literal_constant
  :  T_PERIOD_EXPONENT (T_UNDERSCORE kind_param)? 
     -> ^(T_PERIOD_EXPONENT kind_param?)
  ;
   
// $>

// $<4.4.4 Character Type

// R424
char_selector
  :  T_ASTERISK char_length (T_COMMA)? -> ^(T_CHAR_SELECTOR ^(T_KIND char_length) )
  
  |  T_LPAREN (fp=T_KIND | fp=T_LEN) T_EQUALS ftp=type_param_value
     (T_COMMA (sp=T_KIND | sp=T_LEN) T_EQUALS stp=type_param_value)? T_RPAREN 
       -> ^(T_CHAR_SELECTOR  ^($fp $ftp) (^($sp $stp))?)
  
  |  T_LPAREN type_param_value (T_COMMA (T_KIND T_EQUALS)? expr T_RPAREN)?  
      -> ^(T_CHAR_SELECTOR ^(T_LEN type_param_value) (^(T_KIND expr))?)
  ;

// R425
length_selector
  :  T_LPAREN ( T_LEN T_EQUALS )? type_param_value T_RPAREN -> type_param_value
  |  T_ASTERISK char_length (T_COMMA)? -> char_length
  ;

// R426
char_length
  :  T_LPAREN type_param_value T_RPAREN -> type_param_value
  |  int_literal_constant
  ;


//R427	
char_literal_constant
  : (T_DIGIT_STRING T_UNDERSCORE)? T_CHAR_CONSTANT -> ^(T_CHAR_CONSTANT T_DIGIT_STRING?)
  |  T_IDENT T_CHAR_CONSTANT -> ^(T_CHAR_CONSTANT T_IDENT) 
  ;

// $>

// $>

// $<4.5 Derived types

// $<4.5.1 Derived-type definition

// R429
derived_type_def
  :  label? derived_type_stmt private_or_sequence* component_part?  end_type_stmt
     -> ^(T_DERIVED_TYPE_DEF derived_type_stmt private_or_sequence* component_part? end_type_stmt)
  ;

// R430
derived_type_stmt
  :  T_TYPE (type_attr_spec_list? T_COLON_COLON)? T_IDENT end_of_stmt
     -> ^(T_DERIVED_TYPE_STMT  T_IDENT type_attr_spec_list?)
  ;
  
type_attr_spec_list
  : type_attr_spec (T_COMMA type_attr_spec)*
    -> type_attr_spec+
  ;  
  

// R431
type_attr_spec
  :  access_spec
  |  T_EXTENDS T_LPAREN T_IDENT T_RPAREN -> ^(T_EXTENDS T_IDENT)
  |  T_ABSTRACT
  |  T_BIND T_LPAREN T_IDENT T_RPAREN -> ^(T_BIND T_IDENT)
  ;

// R432
private_or_sequence
  :  T_PRIVATE end_of_stmt -> T_PRIVATE
  |  T_SEQUENCE end_of_stmt -> T_SEQUENCE
  ;

// R433
end_type_stmt
  :  label? T_END T_TYPE T_IDENT? end_of_stmt 
       -> ^(T_ENDTYPE[$T_END] T_IDENT?)

  |  label? T_ENDTYPE T_IDENT? end_of_stmt 		
       -> ^(T_ENDTYPE T_IDENT?)
  ;

// $>

// $<4.5.3 Components

// R438
component_part
  :  data_component_def_stmt+
    //data_component_def_stmt+
  ;
  
// R440
data_component_def_stmt
  : label? declaration_type_spec ( ( T_COMMA attr_spec )* T_COLON_COLON )? 
  	component_decl* end_of_stmt
      -> ^(T_DATA_COMP_DEF_STMT declaration_type_spec attr_spec* component_decl+) 		
    ;
   
// R442
component_decl
  :  T_IDENT (T_LPAREN array_spec T_RPAREN)? ( T_ASTERISK char_length)?  initialization?
      -> ^(T_COMP_DECL T_IDENT array_spec? char_length? initialization?)
  ;


// $<4.5.8 Derived-type specifier

// R455
derived_type_spec
  :  T_IDENT
  ;
  
type_param_spec_list
  :  type_param_spec( T_COMMA type_param_spec)*
      -> type_param_spec+
  ;

// R456
type_param_spec
  :  ( name T_EQUALS)? type_param_value
      -> ^(T_TYPE_PARAM_SPEC  name? type_param_value)
  ;


// $>


// $<4.5.9 Construction of derived-type values

structure_constructor
  :  T_IDENT (T_LPAREN type_param_spec_list? T_RPAREN)
      -> ^(T_STRUCTURE_CTOR T_IDENT type_param_spec_list?)
  ;

// $>


// $>



// $<4.7 Construction of array values

// R465
array_constructor
  :  (T_LPAREN T_SLASH ac_spec T_SLASH T_RPAREN |  T_LBRACKET ac_spec T_RBRACKET) 
       -> ^(T_ARRAY_CONSTRUCTOR ac_spec)
  ;

// R466
ac_spec
  : type_spec T_COLON_COLON (ac_value_list)? -> ^(T_AC_SPEC type_spec ac_value_list?)
  | ac_value_list -> ^(T_AC_SPEC ac_value_list)
  ;	
    
// R469
ac_value
options {backtrack=true;}
  :  expr
  |  ac_implied_do
  ;

ac_value_list
  :  ac_value ( T_COMMA ac_value)* -> ^(T_AC_VALUE_LIST ac_value+)
  ;

// R470
ac_implied_do
  :  T_LPAREN ac_value_list T_COMMA ac_implied_do_control T_RPAREN
     -> ^(T_AC_INPLIED_DO ac_value_list ac_implied_do_control)
  ;    

// R471
ac_implied_do_control
  :  T_IDENT T_EQUALS e1=expr T_COMMA e2=expr ( T_COMMA e3=expr)?
     -> ^(T_AC_IMPLIED_DO_CONTROL $e1 $e2 $e3?)
  ;

// $>

// $>

// $>




// $<Section 5

// $<5.1 Type declaration statements

// R501
type_declaration_stmt
  : label ? declaration_type_spec ( (T_COMMA  attr_spec )* T_COLON_COLON )?
    entity_decl (T_COMMA entity_decl)* end_of_stmt    	
    -> ^(T_TYPE_DECLARATION_STMT declaration_type_spec attr_spec* entity_decl+) 
  ;

// R502
declaration_type_spec
  :  intrinsic_type_spec  
  |  T_TYPE T_LPAREN  derived_type_spec T_RPAREN
      -> ^(T_TYPE derived_type_spec)
/*	|	T_CLASS	T_LPAREN derived_type_spec T_RPAREN
			{ action.declaration_type_spec($T_CLASS, 
                IActionEnums.DeclarationTypeSpec_CLASS); }
	|	T_CLASS T_LPAREN T_ASTERISK T_RPAREN
			{ action.declaration_type_spec($T_CLASS,
                IActionEnums.DeclarationTypeSpec_unlimited); }*/
  ;
	
// R503
attr_spec
  :  access_spec
  |  T_ALLOCATABLE
  |  T_ASYNCHRONOUS
  |  T_DIMENSION T_LPAREN array_spec T_RPAREN// -> array_spec
  |  T_EXTERNAL
	/*|	T_INTENT T_LPAREN intent_spec T_RPAREN*/

  |  T_INTRINSIC
	/*|	language_binding_spec	*/

  |  T_OPTIONAL
  |  T_PARAMETER
  |  T_POINTER
  |  T_PROTECTED
  |  T_SAVE
  |  T_TARGET
  |  T_VALUE
  |  T_VOLATILE
  ;


// R504
entity_decl
  :  T_IDENT (T_LPAREN array_spec T_RPAREN)? 
     (T_ASTERISK char_length )? ( initialization )?
     -> ^(T_ENTITY_DECL T_IDENT array_spec? char_length? initialization?)
  ;	

// R506
initialization
  :  T_EQUALS expr -> ^(T_EQUALS expr)
      //|	T_EQ_GT null_init	{ action.initialization(false, true); }
  ;


// R508
access_spec 
  :  T_PUBLIC
  |  T_PRIVATE
  ;

// R510
array_spec
  :  array_spec_element
     (T_COMMA array_spec_element)*
     -> ^(T_ARRAY_SPEC array_spec_element+)
  ;

array_spec_element
  :
     expr (T_COLON (expr? | T_ASTERISK ) ) -> ^(T_ARRAY_SPEC_ELEMENT expr expr?)
  |  T_ASTERISK -> ^(T_ARRAY_SPEC_ELEMENT T_ASTERISK)
  |  T_COLON -> ^(T_ARRAY_SPEC_ELEMENT T_COLON)
  ;

// $>

// $>

// $<Section 6

variable
  :  designator
  ;

// designator is object-name
// or array-element
// or array-section
// or structure-component
// or substring

// R603
designator	
  :  data_ref (T_LPAREN substring_range T_RPAREN)?
     -> ^(T_DEGIGNATOR data_ref substring_range?)
  |  char_literal_constant T_LPAREN substring_range T_RPAREN
     -> ^(T_DEGIGNATOR char_literal_constant substring_range)
  ;
	
// R611
substring_range
  :  expr? T_COLON expr?
  ;
	
// R612
data_ref
  :  part_ref (T_PERCENT part_ref)* -> ^(T_DATA_REF part_ref+)
  ;
		
part_ref
  : (T_IDENT T_LPAREN) => T_IDENT T_LPAREN section_subscript_list T_RPAREN
     -> ^(T_PART_REF T_IDENT section_subscript_list) 			
  |  T_IDENT -> ^(T_PART_REF T_IDENT)
  ;

section_subscript_list
  :  section_subscript (T_COMMA section_subscript)*       
  ;

//R619 
section_subscript 
  :  e1=expr (T_COLON e2=expr? (T_COLON e3=expr)?)?
  |  T_COLON e2=expr? (T_COLON e3=expr)?
  ;

// $>


// $<Section 7

// $<7.1 Expressions

/*
 * Section 7:
 */

// R722
// R701
primary    
  ://	designator_or_func_ref
     literal_constant
  |  designator
  |  array_constructor
	//|	structure_constructor
  |  T_LPAREN expr T_RPAREN -> expr
  ;

// R702
level_1_expr
  :  T_DEFINED_OP? primary      
  ;


// R704
mult_operand
  :  level_1_expr (T_POWER^ mult_operand)?
  ;

// R705
add_operand
  :  mult_operand ((T_ASTERISK | T_SLASH)^ mult_operand)*
  ;

// R706
level_2_expr
  :   T_PLUS add_operand -> add_operand
  |   T_MINUS^ add_operand
  |   add_operand ((T_PLUS | T_MINUS)^ add_operand)*    
  ;

// R710
level_3_expr
  :  level_2_expr (T_SLASH_SLASH^ level_2_expr)*
  ;
  
// R712
level_4_expr
  :  level_3_expr (rel_op^ level_3_expr)?
  ; 

// R713
rel_op
  :  T_EQ 		 	
  |  T_NE 		 	
  |  T_LT		 	
  |  T_LE		 	
  |  T_GT 		 	
  |  T_GE 		 	
  |  T_EQ_EQ 	 	
  |  T_SLASH_EQ 	 	
  |  T_LESSTHAN 		
  |  T_LESSTHAN_EQ 	
  |  T_GREATERTHAN 	
  |  T_GREATERTHAN_EQ
  ;

// R714
and_operand
  :  level_4_expr
  |  ^(T_NOT level_4_expr)
  ;

// R715
or_operand
  :  and_operand (T_AND^ and_operand)* 
  ;

// R716
equiv_operand
  :  or_operand ((T_EQV | T_NEQV)^ or_operand)*  
  ;

// R717
level_5_expr
  :  (equiv_operand->equiv_operand) (T_DEFINED_OP operand=equiv_operand 
  	-> ^(T_DEFINED_BINARY_OP[$T_DEFINED_OP] $level_5_expr $operand))*
  ;

// R722
expr
  :  level_5_expr
  ;
  
    
// R734
assignment_stmt
  :  variable T_EQUALS expr end_of_stmt -> ^(T_ASSIGNMENT_STMT variable expr)
  ;      
    
// $>

// $>

// $<Section 8

// R801
block
  :  execution_part_construct*
  ;

// $<8.1.2 IF construct

// R802
if_construct
  :  if_then_stmt block (else_if_stmt block)* ( else_stmt block )? end_if_stmt    
  ;

// R803
if_then_stmt
  :  ( T_IDENT T_COLON)? T_IF T_LPAREN expr T_RPAREN T_THEN end_of_stmt
     -> ^(T_IF_THEN expr T_IDENT?)
  ;

// R804
else_if_stmt
  :  label? T_ELSE T_IF T_LPAREN expr T_RPAREN T_THEN T_IDENT? end_of_stmt
     -> ^(T_ELSEIF[$T_ELSE] expr T_IDENT?)
  |  label? T_ELSEIF T_LPAREN expr T_RPAREN T_THEN T_IDENT? end_of_stmt		
     -> ^(T_ELSEIF expr T_IDENT?)  
  ;

// R805
else_stmt
  :  label? T_ELSE T_IDENT? end_of_stmt
     -> ^(T_ELSE T_IDENT?)
  ;

// R806
end_if_stmt
  :  label? T_END T_IF T_IDENT? end_of_stmt
     -> ^(T_ENDIF[$T_END] T_IDENT?)
  |  label? T_ENDIF T_IDENT? end_of_stmt	
     -> ^(T_ENDIF T_IDENT?)  
  ;

// R807
if_stmt
  :  T_IF_STMT T_IF T_LPAREN expr T_RPAREN action_stmt
    -> ^(T_IF_CONSTRUCT expr action_stmt T_ENDIF)
  ;

// $>

// $<8.1.6 DO construct

// R825
do_construct
  :  do_stmt block end_do
     -> ^(T_DO_CONSTRUCT do_stmt block end_do)
  ;

// R827
do_stmt
  :  ( T_IDENT T_COLON)? T_DO label? loop_control? end_of_stmt
      -> ^(T_DO T_IDENT? label? loop_control?)
  ;

// R829 inlined in R827
loop_control
  : T_COMMA? T_WHILE T_LPAREN expr T_RPAREN
      -> ^(T_WHILE expr) 
    | ( T_COMMA )? do_variable T_EQUALS e1=expr T_COMMA e2=expr (T_COMMA e3=expr)?     
      -> ^(T_FOR_LOOP_CONTROL[$T_EQUALS] do_variable $e1 $e2 $e3?)
    ;

// R831
do_variable
  :  variable  
  ;

// R833
end_do
options {backtrack = true; }
  :  end_do_stmt
  |  do_term_action_stmt
  ;

// R834
end_do_stmt
  :  label? T_END T_DO T_IDENT? end_of_stmt 
      -> ^(T_ENDDO[$T_END] T_IDENT? label?)
      
  |  label? T_ENDDO T_IDENT? end_of_stmt  
      -> ^(T_ENDDO T_IDENT? label?)
  ;

do_term_action_stmt
  :  label (   action_stmt 
                   -> ^(T_DO_TERM_ACTION_STMT action_stmt label)
           | ( ((a=T_END T_DO | a=T_ENDDO) T_IDENT?) 
                   -> ^(T_ENDDO[$a] T_IDENT? label) )
     end_of_stmt)
          
  ;
  
  
// R843
cycle_stmt
  :  T_CYCLE T_IDENT? end_of_stmt
      -> ^(T_CYCLE T_IDENT?)
  ;

// R844
exit_stmt
  :  T_EXIT T_IDENT? end_of_stmt
      -> ^(T_EXIT T_IDENT?)
  ;

// $>


// $>


// $<11.1 Main program

// R1101
main_program
  :  program_stmt specification_part execution_part end_program_stmt
     -> ^(T_MAIN_PROGRAM program_stmt specification_part execution_part end_program_stmt)
  ;


// R1102
program_stmt
  :  label? T_PROGRAM T_IDENT end_of_stmt 
     -> ^(T_PROGRAM T_IDENT)
  ;

// R1103
end_program_stmt
  :  label? ( p=T_END T_PROGRAM T_IDENT end_of_stmt
	    | p=T_END end_of_stmt
	    | p=T_ENDPROGRAM T_IDENT end_of_stmt)
     -> ^(T_ENDPROGRAM[$p] T_IDENT?)
  ;	




end_of_stmt
  :  T_NEWLINE+ 	-> 
  |  T_EOS T_NEWLINE*  	->
  |  EOF 	      	->

  ; 
